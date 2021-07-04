import {PLATFORM} from 'aurelia-pal';
import {Router, RouterConfiguration} from 'aurelia-router';
import { ValidationMessageProvider } from 'aurelia-validation';
import { I18N } from 'aurelia-i18n';
import {autoinject} from 'aurelia-framework';

import { SettingsService } from './services/settings-service';
import { SharedValuesService } from 'services/shared-values-service';

@autoinject
export class App {
  public router: Router;

  constructor(private i18n: I18N, private settingsService: SettingsService, private sharedValuesService: SharedValuesService) {
      ValidationMessageProvider.prototype.getMessage = function (key) {
        const translation = i18n.tr(`errorMessages.${key}`);
        return this.parser.parse(translation);
      };

      ValidationMessageProvider.prototype.getDisplayName = function (propertyName: string, displayName: string) {
        return i18n.tr(displayName ? displayName : propertyName);
      };
  }

  public configureRouter(config: RouterConfiguration, router: Router): Promise<void> | PromiseLike<void> | void {
    config.title = 'Hahn Applicaton';
    config.map([
      {
        route: [''],
        name: 'asset',
        moduleId: PLATFORM.moduleName('./pages/assets/index'),
        nav: true,
        title: this.i18n.tr("keyTitleAssets")
      },
      {
        route: ['confirm'],
        name: 'confirm',
        moduleId: PLATFORM.moduleName('./pages/confirm/index'),
        nav: false,
        title: this.i18n.tr("keyThankYou")
      },
      // {
      //   route: 'users',
      //   name: 'users',
      //   moduleId: PLATFORM.moduleName('./users'),
      //   nav: true,
      //   title: 'Github Users'
      // }
    ]);

    this.router = router;
  }
}
