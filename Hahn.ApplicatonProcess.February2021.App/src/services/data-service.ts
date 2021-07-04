import {autoinject} from 'aurelia-framework';
import { SettingsService } from './settings-service';
import { HttpClient, HttpResponseMessage } from 'aurelia-http-client';

@autoinject
export class DataService {

    constructor(private client: HttpClient, private settingService:SettingsService) {

    }

    public GetAssets(page: Number, itemsPerPage: Number) : Promise<HttpResponseMessage> {
      return this.client.get(this.settingService.defaultAssetApiEndpoint, { page, itemsPerPage});
    }

    public GetAsset(id: Number) : Promise<HttpResponseMessage> {
      return this.client.get(`${this.settingService.defaultAssetApiEndpoint}/${id}`);
    }

    public SendAsset(asset: AssetEditDto): Promise<HttpResponseMessage> {
        return this.client.post(this.settingService.defaultAssetApiEndpoint, asset);
    }

    public UpdateAsset(asset: AssetEditDto): Promise<HttpResponseMessage> {
      return this.client.put(this.settingService.defaultAssetApiEndpoint, asset);
  }

    public DeleteAsset(id: Number): Promise<HttpResponseMessage> {
      return this.client.delete(this.settingService.defaultAssetApiEndpoint + `/${id}`);
    };
}

export class Asset {
    public assetName: string;
    public department: string;
    public country: string;
    public purchaseDate: string;
    public email: string;
    public broken: boolean;
}

export class AssetEditDto extends Asset {
    public id: number;
}

export class Country
{
  public name: string;
  public alpha3Code: string;
  public translations: Map<string,string>;
  public translation: string;
}
