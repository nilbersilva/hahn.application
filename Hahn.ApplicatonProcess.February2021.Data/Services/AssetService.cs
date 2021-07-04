using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Hahn.ApplicatonProcess.February2021.Data.Utils;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using System.Linq.Expressions;

namespace Hahn.ApplicatonProcess.February2021.Data.Services
{
    public class AssetService : IAssetService
    {
        private readonly IMapper _mapper;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssetService(IMapper mapper, IAssetRepository assetRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CreateAsync(AssetDto asset)
        {
            var newItem = _mapper.Map<Asset>(asset);
            newItem.CreatedAt = DateTime.UtcNow;
            newItem.CreatByIPAddress = _httpContextAccessor.GetIPAddress();
            _assetRepository.Add(newItem);
            await _unitOfWork.CommitAsync();
            return newItem.ID;
        }

        public async Task<bool> UpdateAsync(AssetDto asset)
        {
            var existing = await _assetRepository.GetAsync(x => x.ID == asset.ID);
            if (asset.ID.HasValue && existing is not null)
            {
                var toUpdate = _mapper.Map<Asset>(asset);
                existing.MatchPropertiesFrom(toUpdate);

                existing.ModifiedAt = DateTime.UtcNow;
                existing.CreatByIPAddress = _httpContextAccessor.GetIPAddress();

                _assetRepository.Update(existing);
                await _unitOfWork.CommitAsync();
                return true;
            }
            throw new NotFoundException(asset.ID.ToString());
        }

        public async Task<AssetDto> GetAsync(int id)
        {
            var result = await _assetRepository.GetAsync(x => x.ID == id);
            if (result is null)
            {
                throw new NotFoundException(id.ToString());
            }
            return _mapper.Map<AssetDto>(result);
        }

        public async Task DeleteAsync(int id)
        {
            var toDelete = await _assetRepository.GetAsync(x => x.ID == id);
            if (toDelete == null)
            {
                throw new NotFoundException(id.ToString());
            }
            _assetRepository.Delete(toDelete);
            await _unitOfWork.CommitAsync();
        }

        public IEnumerable<AssetDto> GetByPage(Expression<Func<Asset, bool>> where, int page = 0, int itemsPerPage = 10)
        {
            var result = _assetRepository.GetByPage(where, page, itemsPerPage);
            return _mapper.Map<IEnumerable<AssetDto>>(result);
        }

        public async Task<long> CountAsync(Expression<Func<Asset, bool>> where)
        {
            return await _assetRepository.CountAsync(where);
        }
    }
}
