
import * as locale from 'locale-codes';

var mode   = process.env.NODE_ENV;

export class SettingsService {
  public defaultLanguage: string;
  public defaultLocale: string;
  public defaultDateFormat: string = "yyyy/MM/dd";
  public defaultAPiEndpoint: string = mode === "development" ? "https://localhost:5001/" : "";
  public defaultAssetApiEndpoint: string = this.defaultAPiEndpoint + "api/Asset";
  public defaultSharedValuesServiceApiDepartmentsEndpoint: string =  this.defaultAPiEndpoint + "api/SharedValues/GetDepartments";
  public defaultSharedValuesServiceApiCountriesEndpoint: string =  this.defaultAPiEndpoint + "api/SharedValues/GetCountries";
  public defaultSharedValuesServiceApiTopLevelDomainsEndpoint: string =  this.defaultAPiEndpoint + "api/SharedValues/GetTopLevelDomains";
  public storageKeyDepartments: string = "Departments";
  public storageKeyCountries: string = "Countries";
  public storageKeyTopLevelDomains: string = "TopLevelDomains";

  constructor() {    
    var lang = navigator.language;
    if (navigator.languages != undefined) 
    {
      lang = navigator.languages[0]; 
    }

    const region = locale.getByTag(lang);       
    var translation = region['iso639-1']; 
    this.defaultLocale = translation;
    this.defaultLanguage = lang;
  }
}
