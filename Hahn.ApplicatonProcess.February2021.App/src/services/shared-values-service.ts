import { HttpClient, HttpResponseMessage } from 'aurelia-http-client';
import {autoinject} from 'aurelia-framework';
import { SettingsService } from './settings-service';
import {Country} from './data-service';

@autoinject
export class SharedValuesService {

  constructor(private client: HttpClient,
    private settingService: SettingsService) {
  }

    get departments(): Map<string, string> {
        let text = localStorage.getItem(this.settingService.storageKeyDepartments);
        if (text) {
            let raw = JSON.parse(text) as Array<any>;
            let result = new Map<string, string>();
            raw.forEach((x) => result.set(x.id.toString(), x.departmentName));
            return result;
        }
        return new Map<string, string>();
    }

    get countries(): Map<string, Country> {
        let text = localStorage.getItem(this.settingService.storageKeyCountries);
        if (text) {
            let raw = JSON.parse(text) as Array<Country>;
            let result = new Map<string, Country>();
            raw.forEach((x) => {
              var newObj = {...x, translation: x.translations[this.settingService.defaultLocale] || x.name};
              result.set(x.alpha3Code, newObj)
            });
            return result;
        }
        return new Map<string, Country>();
    }

    get domains(): Array<string> {
        let text = localStorage.getItem(this.settingService.storageKeyTopLevelDomains);
        if (text) {
            let raw = JSON.parse(text) as Array<string> ;
            return raw;
        }
        return new Array<string>();
    }

    populateDepartments(): Promise<HttpResponseMessage> {
        return this.client.get(this.settingService.defaultSharedValuesServiceApiDepartmentsEndpoint);
    }

    populateCountries(): Promise<HttpResponseMessage> {
        return this.client.get(this.settingService.defaultSharedValuesServiceApiCountriesEndpoint);
    }

    populateTopLevelDomains(): Promise<HttpResponseMessage> {
        return this.client.get(this.settingService.defaultSharedValuesServiceApiTopLevelDomainsEndpoint);
    }

    async loadDepartments() {
      if (this.departments.size == 0)
      {
        var request = await this.populateDepartments();
        if(request.isSuccess) {
          var response = JSON.parse(request.response);
          localStorage.setItem(this.settingService.storageKeyDepartments, JSON.stringify(response.departments));
        }
      }
    }
    
    async loadCountries() {
      if (this.countries.size == 0)
      {
        var request = await this.populateCountries();
        if(request.isSuccess) {
          var response = JSON.parse(request.response).countries as Array<Country>;
    
          localStorage.setItem(this.settingService.storageKeyCountries, JSON.stringify(response));
        }
      }
    }

    async loadTopLevelDomains() {
      if (this.domains.length == 0)
      {
        var request = await this.populateTopLevelDomains();
        if(request.isSuccess) {
          var response = JSON.parse(request.response);
          localStorage.setItem(this.settingService.storageKeyTopLevelDomains, JSON.stringify(response.topLevelDomains));
        }
      }
    }

   getLang() {
      if (navigator.languages != undefined) 
        return navigator.languages[0]; 
      return navigator.language;
    }
}
