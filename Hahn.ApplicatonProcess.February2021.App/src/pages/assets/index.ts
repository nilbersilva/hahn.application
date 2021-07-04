import {autoinject, computedFrom, bindable, PLATFORM, useView} from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { Router } from 'aurelia-router';
import { ValidationController, } from 'aurelia-validation';
import { HttpResponseMessage } from 'aurelia-http-client';
import { format } from 'date-fns'
import * as $ from 'jquery';
import * as locale from 'locale-codes'
import * as moment from 'moment';

import { SharedValuesService } from './../../services/shared-values-service';
import { SettingsService } from '../../services/settings-service';
import { DataService, AssetEditDto } from '../../services/data-service';
import { DialogModel, DialogStyle, Prompt } from '../../components/prompt';
import { AssetEdit, activateParams } from './edit';

@autoinject
export class Assets {
  @bindable public assets: AssetEditDto[];

  @bindable public dateFormat;

  @bindable public isBusy: boolean = false;

  assetValidationController: ValidationController;
  dialogService: DialogService;
  router: Router;
  dataService: DataService;
  sharedValuesService: SharedValuesService;
  settingService: SettingsService;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalPages: number;
  totalItems: number;
  pages: number[];
 constructor(
    sharedValuesService: SharedValuesService, settingsService: SettingsService, dialogService: DialogService, router: Router, dataService: DataService) 
    {
      this.dateFormat = settingsService.defaultDateFormat;
      this.dialogService = dialogService;
      this.router = router;
      this.dataService = dataService;
      this.settingService = settingsService;
      this.sharedValuesService = sharedValuesService;
      this.assets = [];
    }

    async attached(): Promise<void> 
    {
        this.currentPage = 1;
        await this.loadAssets(this.currentPage);
    }

    async loadAssets(page: number): Promise<void> {

      this.isBusy = true;

      try {

      if (this.assets == null || this.assets.length == 0) {
        await this.sharedValuesService.loadDepartments();
        await this.sharedValuesService.loadCountries();
      }

      await this.dataService.GetAssets(page , this.itemsPerPage).then((x: HttpResponseMessage) => 
      { 
        if (x.isSuccess) {
          var departments = this.sharedValuesService.departments;
          var countries = this.sharedValuesService.countries;

          var response = JSON.parse(x.response);
          this.totalPages = response.totalPages;
          this.totalItems = response.totalItems;
          this.currentPage = page;
          if(this.totalPages > 0)
          {
            this.pages = this.getPagingRange(page, 1,this.totalPages, 5);
          }
          else {
            this.pages = [];
          }
        
          //this.pages = this.getPageList(this.totalPages, page, 6);
          this.assets = response.assets.map((x: AssetEditDto)=> {       
         
            var country = countries.get(x.country);
            var lang = this.sharedValuesService.getLang();
            const region = locale.getByTag(lang);          
            var translation = country.translations[region['iso639-1']];
          
            if (translation === undefined )
            {
              translation = country.name;
            }
              return {
                    ...x, 
                    formattedDate: format(new Date(x.purchaseDate), this.dateFormat),
                    departmentName: departments.get(x.department),
                    translation : country.translation
                  };
                });

          }
      });

      } catch (error) {
        this.showDefaultErrorPrompt();
      }   
      finally{
        this.isBusy = false;
      }   
    }
    
    async viewAsset(id: Number): Promise<void> {
      await this.assetOperation(id, false);
    }

    async editAsset(id: Number): Promise<void> {
      await this.assetOperation(id, true);
    }

    async createAsset(id: Number): Promise<void> {
      await this.assetOperation(0, true);
    }

    async assetOperation(id: Number, edit: boolean): Promise<void> {
      this.isBusy = true;
      try {
        if (id === 0)
        {     
          this.dialogService.open({viewModel: AssetEdit, lock: edit, model: {undefined, viewOnly: false}, verticalAlignment: "top"  });
        }
        else
        {
          await this.dataService.GetAsset(id).then((req) => {
            if (req.isSuccess) {
                var asset = JSON.parse(req.response) as AssetEditDto;          
                var params = {} as activateParams;
                params.asset = asset;
                params.viewOnly = !edit;
                params.reloadAssets = async ()=>{
                  await this.loadAssets(this.currentPage);
                };
                this.dialogService.open({viewModel: AssetEdit, lock: edit, model: params, verticalAlignment: "top"  });
            }
            else 
            {
              this.showDefaultErrorPrompt();
            }
          });
        }
       
      } catch(error)  {
        var statusCode = 0;
        if(error.statusCode)
        {
          statusCode = error.statusCode;
        }

        if(statusCode === 404)
        {
          let model = new DialogModel();
          model.title = "keyErrorDialogTitle";
          model.message = "keyAssetNotFound";
          model.dialogStyle = DialogStyle.Ok;       
          this.dialogService.open({
                viewModel: Prompt, model: model, lock: false
          });
        }
        else {
          this.showDefaultErrorPrompt();
        }
      }
      finally {
        this.isBusy = false;
      }
    }

    async deleteAsset(id: Number): Promise<void> {
      let model = new DialogModel();

      model.title = "keyDeleteAsset";
      model.message = "keyConfirmDelete";
      model.dialogStyle = DialogStyle.YesNo;
   
      this.dialogService.open({
        viewModel: Prompt, model: model, lock: false
        }).whenClosed(async response => {
            if(response.wasCancelled) return;
            this.isBusy = true;
            try {
              await this.dataService.DeleteAsset(id).then((req) => {
                if (req.isSuccess) {
                  this.dialogService.closeAll();
                    var newAssets = this.assets.filter(asset => asset.id != id);   
                    this.assets  = newAssets;       
                }
                else {
                  let model = new DialogModel();
                  model.title = "keyErrorDialogTitle";
                  model.message = "keyErrorDialogMessage";
                  model.dialogStyle = DialogStyle.YesNo;
                    this.dialogService.open({
                        viewModel: Prompt, model: model, lock: false
                    });
                  }
              });
            } catch (error) {
              var statusCode = 0;
              if(error.statusCode)
              {
                statusCode = error.statusCode;
              }

              if(statusCode === 404)
              {
                let model = new DialogModel();
                model.title = "keyErrorDialogTitle";
                model.message = "keyAssetNotFound";
                model.dialogStyle = DialogStyle.Ok;       
                this.dialogService.open({
                      viewModel: Prompt, model: model, lock: false
                });
              }
              else {
                this.showDefaultErrorPrompt();
              }
            } 
            finally {
              this.isBusy = false;
            }
        });
    }

    range(n) {
      var arr = [];
      while (n > 0) {
          arr.unshift(--n);
      }
      return arr;
    }

   getPageList(totalPages, page, maxLength) : number[] {
      if (maxLength < 5) throw "maxLength must be at least 5";
  
      function range(start, end) {
          return Array.from(Array(end - start + 1), (_, i) => i + start); 
      }
  
      var sideWidth = maxLength < 9 ? 1 : 2;
      var leftWidth = (maxLength - sideWidth*2 - 3) >> 1;
      var rightWidth = (maxLength - sideWidth*2 - 2) >> 1;
      if (totalPages <= maxLength) {
          // no breaks in list
          return range(1, totalPages);
      }
      if (page <= maxLength - sideWidth - 1 - rightWidth) {
          // no break on left of page
          return range(1, maxLength - sideWidth - 1)
              .concat(0, range(totalPages - sideWidth + 1, totalPages));
      }
      if (page >= totalPages - sideWidth - 1 - rightWidth) {
          // no break on right of page
          return range(1, sideWidth)
              .concat(0, range(totalPages - sideWidth - 1 - rightWidth - leftWidth, totalPages));
      }
      // Breaks on both sides
      return range(1, sideWidth)
          .concat(0, range(page - leftWidth, page + rightWidth),
                  0, range(totalPages - sideWidth + 1, totalPages));
    }

   getPagingRange(current, min = 1, total = 20, length = 5) {
      if (length > total) length = total;
    
      let start = current - Math.floor(length / 2);
      start = Math.max(start, min);
      start = Math.min(start, min + total - length);
     
      return Array.from({length: length}, (el, i) => start + i);
    }

    showDefaultErrorPrompt(){
      let model = new DialogModel();
            model.title = "keyErrorDialogTitle";
            model.message = "keyErrorDialogMessage";
            model.dialogStyle = DialogStyle.Ok;       
            this.dialogService.open({
                  viewModel: Prompt, model: model, lock: false
            });
    }
}
