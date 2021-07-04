import {customAttribute, inject, bindable, dynamicOptions} from 'aurelia-framework'
import {ValidationController} from 'aurelia-validation'
import * as $ from "jquery";

@dynamicOptions
@inject(Element)
@customAttribute('disabled-if-not-valid')
export class DisabledIfNotValid {
    @bindable({ primaryProperty: true }) value;
    private controller: ValidationController;
    constructor(private element: Element) {
      $(element).prop('disabled',true);
    }

    valueChanged() {
      console.log('changed')
        let errorCount = this.controller.errors.length;
        if (errorCount === 0) {
            this.controller.validate();
        }
       $(this.element).prop('disabled', errorCount !== 0);
    }

    propertyChanged(name, newValue, oldValue){
      let errorCount = this.controller.errors.length;
      if (errorCount === 0) {
            this.controller.validate();
      }
      $(this.element).prop('disabled', errorCount !== 0);
    }

    bind(bindingContext:{controller:ValidationController}) {
      this.controller = bindingContext.controller;
    }
}
