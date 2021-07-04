import { inject, customAttribute } from 'aurelia-framework';
import * as $ from 'jquery';
import { SettingsService } from '../../../services/settings-service';
import * as moment from 'moment';
import { format } from 'date-fns'

@customAttribute('datepicker')
@inject(Element, SettingsService)
export class DatePicker {
    constructor(private element: Element, private settingsService: SettingsService) {
    }

    attached() {      
        ($(this.element) as any).datepicker({
            format: this.settingsService.defaultDateFormat.toLowerCase(),
            locale: this.settingsService.defaultLanguage.toLowerCase(),
            todayHighlight: true,
            language: this.settingsService.defaultLanguage.toLowerCase(),
            autoclose:true
        }).on("hide", (e) => { 
            if (e.target.value && e.target.value.length > 0) {
                const event = new Event('change');
                e.target.dispatchEvent(event);
            }
        }).on("changeDate", (e) => {
            if (e.target.value && e.target.value.length > 0) {
                const event = new Event('change');
                e.target.dispatchEvent(event);
            }
        });
        if($(this.element).val()){
          var val = $(this.element).val() as string;
          $(this.element).val(format(new Date(val) ,this.settingsService.defaultDateFormat));
        }
    }
}
