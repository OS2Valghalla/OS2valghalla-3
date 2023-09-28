import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { TranslocoService } from '@ngneat/transloco';

@Directive({ selector: '[appFormError]' })
export class FormErrorDirective implements OnInit {
  readonly id = crypto.randomUUID();

  private readonly errorLocalizations = {
    required: 'shared.validation.required',
    emailInvalid: 'shared.validation.email'
  };

  private currrentErrors: string[] = [];

  constructor(
    private readonly renderer: Renderer2,
    private readonly elementRef: ElementRef<HTMLElement>,
    private readonly translocoService: TranslocoService,
  ) {}

  ngOnInit(): void {
    this.setAttribute('id', this.id);
    this.setAttribute('hidden', '');
    this.addClass('form-error-message');
  }

  show(errors: ValidationErrors) {
    this.removeAttribute('hidden');

    Object.keys(errors).forEach((error) => {
      if (this.currrentErrors.includes(error)) {
        return;
      }

      this.currrentErrors.push(error);

      const errorLocalization = this.errorLocalizations[error] ?? error;
      this.addErrorElement(errorLocalization);
    });
  }

  hide() {
    this.setAttribute('hidden', '');
    this.removeErrorElements();
  }

  setAttribute(name: string, value: string) {
    this.renderer.setAttribute(this.elementRef.nativeElement, name, value);
  }

  removeAttribute(name: string) {
    this.renderer.removeAttribute(this.elementRef.nativeElement, name);
  }

  addClass(name: string) {
    this.renderer.addClass(this.elementRef.nativeElement, name);
  }

  removeClass(name: string) {
    this.renderer.removeClass(this.elementRef.nativeElement, name);
  }

  private addErrorElement(errorLocalization: string) {
    const span = this.renderer.createElement('span');
    this.renderer.addClass(span, 'sr-only');

    const errorTextAccessibility = this.renderer.createText(
      this.translocoService.translate('shared.accessibility.error') + ': ',
    );
    this.renderer.appendChild(span, errorTextAccessibility);

    const errorText = this.renderer.createText(this.translocoService.translate(errorLocalization));
    const br = this.renderer.createElement('br');

    this.renderer.appendChild(this.elementRef.nativeElement, span);
    this.renderer.appendChild(this.elementRef.nativeElement, errorText);
    this.renderer.appendChild(this.elementRef.nativeElement, br);
  }

  private removeErrorElements() {
    this.currrentErrors = [];

    this.elementRef.nativeElement.childNodes.forEach((node) => {
      this.renderer.removeChild(this.elementRef.nativeElement, node);
    });
  }
}
