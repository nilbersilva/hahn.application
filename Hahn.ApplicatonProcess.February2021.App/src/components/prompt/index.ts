import { DialogController } from 'aurelia-dialog';
import { bindable, inject } from 'aurelia-framework';

export enum DialogStyle {
    YesNo,
    Ok
}

export class DialogModel {
    public title: String;
    public message: String;
    public dialogStyle: DialogStyle;
}

@inject(DialogController)
export class Prompt {

    @bindable public config:DialogModel;
    dialogController: DialogController; 

    constructor(dialogController: DialogController) {
        this.dialogController = dialogController;
    }

    activate(config: DialogModel) {
        this.config = config;
    }

    get isYesNoDialog(): boolean {
        return this.config.dialogStyle == DialogStyle.YesNo;
    }

    get isOkDialog(): boolean {
        return this.config.dialogStyle == DialogStyle.Ok;
    }

    Ok() {
        this.dialogController.ok(this.config);
    }

    Cancel() {
        this.dialogController.cancel(false);
    }
}
