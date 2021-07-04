import { bindable, computedFrom, PLATFORM, autoinject } from 'aurelia-framework';
import { inject, NewInstance } from 'aurelia-dependency-injection';
import { ValidationController, ValidationRules,  ControllerValidateResult  } from 'aurelia-validation';
import { Router } from 'aurelia-router';
import { DialogService, DialogController } from 'aurelia-dialog';
import { format } from 'date-fns'
import * as moment from 'moment';
import * as $ from 'jquery';

import { DialogModel, DialogStyle, Prompt } from './../../../components/prompt';
import { SettingsService } from '../../../services/settings-service';
import { SharedValuesService } from '../../../services/shared-values-service';
import { BootstrapFormRenderer } from '../../../renderers/BootstrapFormRenderer';
import { DataService, Asset, Country, AssetEditDto } from '../../../services/data-service';
import * as toastr from "toastr"

export interface activateParams {
  asset: AssetEditDto, 
  viewOnly: boolean, 
  reloadAssets: () => Promise<void>
}

@inject(NewInstance.of(ValidationController), SharedValuesService, DialogController, NewInstance.of(BootstrapFormRenderer), 
        SettingsService, DialogService, Router, DataService)
export class AssetEdit {

  @bindable public title: string;
  @bindable public asset: AssetEditDto;
  @bindable public departments: Map<string, string>;
  @bindable public countries: Map<string, Country> ;
  @bindable public dateFormat;
  @bindable public allowEdit: boolean;
  @bindable public isBusy: boolean;
  @bindable controller: ValidationController;
  @bindable buttonActive: boolean = false;
  @bindable showSucess: boolean = false;

  dialogController: DialogController; 
  assetCopy: AssetEditDto;
  dialogService: DialogService;
  router: Router;
  dataService: DataService;
  sharedValuesService: SharedValuesService;
  settingService: SettingsService;

  private rules: ValidationRules;
  reloadAssets: () => Promise<void>;
  focusableElements = 'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])';

  constructor(controller: ValidationController, sharedValuesService: SharedValuesService,  dialogController: DialogController,
    bootstrapFormRenderer: BootstrapFormRenderer,  settingsService: SettingsService, dialogService: DialogService, 
    router: Router, dataService: DataService) {
      this.controller = controller;
      this.dateFormat = settingsService.defaultDateFormat;
      this.dialogService = dialogService;
      this.router = router;
      this.dataService = dataService;
      this.settingService = settingsService;
      this.sharedValuesService = sharedValuesService;
      this.dialogController = dialogController;

      this.asset = new AssetEditDto();

      this.rules = ValidationRules
      .ensure((res: AssetEditDto) => res.assetName).displayName('keyAssetName').required()
      .ensure((res: AssetEditDto) => res.assetName).minLength(5)
      .ensure((res: AssetEditDto) => res.department).displayName('keyDepartment').required()
      .ensure((res: AssetEditDto) => res.country).displayName('keyCountry').required()
      .ensure((res: AssetEditDto) => res.purchaseDate).displayName('keyPurchaseDate').required()
      .ensure((res: AssetEditDto) => res.purchaseDate).satisfies((value: Date) => { return value ? moment(value, this.dateFormat).isValid() : true }).withMessageKey("invalidValue")
      .ensure((res: AssetEditDto) => res.purchaseDate).displayName('keyPurchaseDate').satisfies((value: Date) => { return value && moment(value, this.dateFormat).isValid() ? moment(value, this.dateFormat).isBetween(moment().subtract(1, "years"), moment()) : true }).withMessageKey("oneYear")
      .ensure((res: AssetEditDto) => res.email).displayName('keyEmail').required()
      .ensure((res: AssetEditDto) => res.email).email()
      .ensure((res: AssetEditDto) => res.email).satisfies((value: string) => {
          let result = false;
          if( !value
              || value.length == 0
              || !/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(value)) return !result;
          let domains = this.sharedValuesService.domains;
          domains.some((domain:string) => {
              result = value.endsWith(domain);
              return result;
          });
          return result;
      }).withMessageKey("wrongTopLevelDomain")
      .rules;

    this.controller.addRenderer(bootstrapFormRenderer);
    this.controller.addObject(this.asset, this.rules );

          
    // this.controller.subscribe( (event: ValidateEvent)=> { 
    //   // var count = this.rules as [];
    //    //this.buttonActive = event.results.length === count.length; 
    //    console.log(     event);
    //    if(event.controllerValidateResult && event.controllerValidateResult.valid)
    //      this.buttonActive = true;
    //     else
    //       this.buttonActive = false;
    //  });
  }

  async activate(params: activateParams) : Promise<void> {
    if(params.viewOnly)
    {
      this.allowEdit = false;
      this.title = "keyTitleAssetView";
      this.controller.removeObject(this.asset);   
      params.asset.purchaseDate = format(new Date(params.asset.purchaseDate) , this.dateFormat);     
      this.asset = params.asset; 
    }
    else 
    {
      this.allowEdit = true;
      if(params.asset)
      {
        this.title = "keyTitleAssetEdit";
        this.controller.removeObject(this.asset);
        params.asset.purchaseDate = format(new Date(params.asset.purchaseDate) , this.dateFormat);     
        this.asset = params.asset;
        this.assetCopy = {...params.asset}     
        this.controller.addObject(this.asset, this.rules );
        this.reloadAssets = params.reloadAssets;
      }
      else 
      {
        this.title = "keyTitleAssetInput";
      } 
    }   
  }

  @computedFrom("asset.assetName", "asset.department", "asset.country", "asset.purchaseDate", "asset.email", "asset.broken")
  get formAnyIsFilled(): boolean {
      return this.asset.assetName != undefined || this.asset.department != undefined || this.asset.country != undefined || 
      this.asset.purchaseDate != undefined || this.asset.email != undefined || this.asset.broken != undefined;
  }

  @computedFrom("asset.assetName", "asset.department", "asset.country", "asset.purchaseDate", "asset.email", "asset.broken","buttonActive")
  get formIsFilled(): boolean {
    try {
        if(this.asset.assetName && this.asset.department && this.asset.country && this.asset.purchaseDate && this.asset.email)
        {
          this.controller.validate().then((result: ControllerValidateResult) => { 
             this.buttonActive = result.valid;
           });
          return this.buttonActive;
        }
        else {
          this.buttonActive = false;
        }
      
      return false;
    } catch (error) {
      console.log(error);
    }  
  
  }

  async attached(): Promise<void> {
   
    let model = new DialogModel();

    model.title = "keyErrorDialogTitle";
    model.message = "keyErrorDialogMessage";
    model.dialogStyle = DialogStyle.Ok;
    this.isBusy = true;
    try 
    {
        await this.sharedValuesService.loadDepartments();
        await this.sharedValuesService.loadCountries();
        await this.sharedValuesService.loadTopLevelDomains();

        this.departments = this.sharedValuesService.departments;
        this.countries = this.sharedValuesService.countries;          
    } catch (error) {
        this.dialogService.open({
            viewModel: Prompt, model: model, lock: false
        });
    }
    finally {
      this.isBusy = false;
    }     
  }

  timeout(ms): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  async submit(): Promise<void> {   
    this.isBusy = true;
    
    try {
      var assetToSend = {...this.asset};
      assetToSend.purchaseDate = moment(assetToSend.purchaseDate).format("YYYY-MM-DDTHH:mm:ssZ");

      //Update
      if(assetToSend.id && assetToSend.id > 0)
      {       
        await this.dataService.UpdateAsset(assetToSend).then(async (request) => {
          if (request.isSuccess) {
            this.isBusy = false;
            this.showSucess = true;
            await this.timeout(1500);
            this.dialogService.closeAll();
            this.reloadAssets().then((response) => {});            
          }
          else
          {
            this.showDefaultErrorPrompt();
          }  
        })
      }
      else
      {
        await this.dataService.SendAsset(assetToSend).then((request) => {
          if (request.isSuccess) {
            this.dialogService.closeAll();               
              this.router.navigateToRoute("confirm");
          }
          else {
            this.showDefaultErrorPrompt();
          }
        });
      }        
    } catch (error) {
      console.log(error);
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

  public reset(): void {

    let model = new DialogModel();
    model.title = "keyResetDialogTitle";
    if(this.asset.id > 0)
    {
      model.message = "keyResetOriginalDialogMessage";
    }
    else {
      model.message = "keyResetDialogMessage";      
    }
    
    model.dialogStyle = DialogStyle.YesNo;

    this.dialogService.open({
        viewModel: Prompt, model: model, lock: false
    }).whenClosed(response => {
        if (!response.wasCancelled) {
            this.clearFormState();
        }
    });
  }

  private clearFormState(): void {
    this.controller.removeObject(this.asset);      

    if( this.asset.id > 0)
    {
      this.asset = {...this.assetCopy};
    }
    else 
    {
      this.asset = new AssetEditDto();
    } 
 
    this.controller.addObject(this.asset, this.rules );

    $("input[type='text']").val('');
    $("input[type='hidden']").val('');
    $("input[type='email']").val('');
    $("input[type='checkbox']").prop('checked', false);

    $("#department-button").text($("#department-button").parent().find(".default-lov-value").text());
    $("#country-button").text($("#country-button").parent().find(".default-lov-value").text());

    $(".is-invalid").removeClass("is-invalid");
    $(".custom-is-invalid").removeClass("custom-is-invalid");
    $(".invalid-feedback ").remove();

    $("input[type='submit']").attr("disabled", "disabled");
    $("input[type='reset']").attr("disabled", "disabled");
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

  public bind(){
    toastr.options.closeButton = true // etc
  }

  public setNotification() {
    toastr.success('teste');
  }

}
