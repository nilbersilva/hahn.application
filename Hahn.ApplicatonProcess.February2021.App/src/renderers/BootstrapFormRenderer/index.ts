import { RenderInstruction, ValidateResult } from 'aurelia-validation';

export class BootstrapFormRenderer {
    render(instruction: RenderInstruction) {

        for (let { result, elements } of instruction.unrender) {
            for (let element of elements) {
                this.remove(element, result);
            }
        }

        for (let { result, elements } of instruction.render) {
            for (let element of elements) {
                this.add(element, result);
            }
        }
    }

    mark(element: Element, result: ValidateResult, addOrRemove:String) {
        const form = element.closest('form');
        let json = form.attributes.getNamedItem("data-map").value;
        if (json) {
            let map = JSON.parse(json) as Array<Map<Number, Number>>;
            let tabindex = element.getAttribute("tabindex");

            if (!map[tabindex]) {
                map[tabindex] = new Array();
            }

            if (addOrRemove == "add") {
                map[tabindex].push(Number(result.valid));
            }
            else if (addOrRemove == "remove") {
                map[tabindex].shift();
            }

            form.attributes.getNamedItem("data-map").value = JSON.stringify(map);
            //console.log(map);
        }
    }

    add(element: Element, result: ValidateResult) {
        //console.log(`add ${element.id} ${result.message} ${result.valid} ${result.id} ${result.rule.messageKey}`);
        //this.mark(element, result, "add");
        const formGroup = element.closest('.form-group');
        if (!formGroup) {
            return;
        }

        if (!result.valid) {
            element.classList.add('is-invalid');

            var toggle = formGroup.querySelector('.dropdown-toggle');

            if (toggle) {
                toggle.classList.add('custom-is-invalid');
            }

            // add help-block
            const message = document.createElement('div');
            message.className = 'invalid-feedback display';
            message.textContent = result.message;
            message.id = `validation-message-${result.id}`;
            formGroup.appendChild(message);
        }
    }

    remove(element: Element, result: ValidateResult) {
        //console.log(`remove ${element.id} ${result.message} ${result.valid} ${result.id} ${JSON.stringify(result.rule.messageKey)}`);
       // this.mark(element, result, "remove");
        const formGroup = element.closest('.form-group');
        if (!formGroup) {
            return;
        }

        if (!result.valid) {
            // remove help-block
            const message = formGroup.querySelector(`#validation-message-${result.id}`);
            if (message) {
                formGroup.removeChild(message);

                // remove the has-error class from the enclosing form-group div
                if (formGroup.querySelectorAll('.invalid-feedback').length === 0) {
                    element.classList.remove('is-invalid');

                    var toggle = formGroup.querySelector('.dropdown-toggle');

                    if (toggle) {
                        toggle.classList.remove('custom-is-invalid');
                    }
                }
            }
        }
    }
}
// import {
//   ValidationRenderer,
//   RenderInstruction,
//   ValidateResult
// } from 'aurelia-validation';

// export class BootstrapFormRenderer {
//   render(instruction: RenderInstruction) {
//     for (let { result, elements } of instruction.unrender) {
//       for (let element of elements) {
//         this.remove(element, result);
//       }
//     }

//     for (let { result, elements } of instruction.render) {
//       for (let element of elements) {
//         this.add(element, result);
//       }
//     }
//   }

//   add(element: Element, result: ValidateResult) {
//     if (result.valid) {
//       return;
//     }

//     const formGroup = element.closest('.form-group');
//     if (!formGroup) {
//       return;
//     }

//     // add the has-error class to the enclosing form-group div
//     formGroup.classList.add('has-error');

//     // add help-block
//     const message = document.createElement('span');
//     message.className = 'help-block validation-message';
//     message.textContent = result.message;
//     message.id = `validation-message-${result.id}`;
//     formGroup.appendChild(message);
//   }

//   remove(element: Element, result: ValidateResult) {
//     if (result.valid) {
//       return;
//     }

//     const formGroup = element.closest('.form-group');
//     if (!formGroup) {
//       return;
//     }

//     // remove help-block
//     const message = formGroup.querySelector(`#validation-message-${result.id}`);
//     if (message) {
//       formGroup.removeChild(message);

//       // remove the has-error class from the enclosing form-group div
//       if (formGroup.querySelectorAll('.help-block.validation-message').length === 0) {
//         formGroup.classList.remove('has-error');
//       }
//     }
//   }
// }


