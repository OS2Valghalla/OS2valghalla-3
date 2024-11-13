import { Component, Input, OnDestroy, OnInit, AfterViewInit, HostBinding, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Editor, NgxEditorComponent, Toolbar } from 'ngx-editor';
import { RichTextEditorOptions } from 'src/shared/models/ux/rich-text-editor-options';

@Component({
  selector: 'app-rich-text-input',
  templateUrl: './rich-text-input.component.html',
  styleUrls: ['./rich-text-input.component.scss'],
  styles: [
    `
      ::ng-deep .NgxEditor__Content {
        min-height: var(--minHeight);
      }
    `,
  ],
})
export class RichTextInputComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() options: RichTextEditorOptions;

  @Input() control: FormControl;

  @Input() readonly?: boolean;

  @Input() hideHeading1?: boolean;

  @HostBinding('style.--minHeight')
  minHeight = '50px';

  toolbar: Toolbar;

  editor: Editor;

  @ViewChild(NgxEditorComponent) private readonly editorComponent;

  ngAfterViewInit(): void {
    if (this.editorComponent) {
      const contentEditors = this.editorComponent.elementRef.nativeElement.getElementsByClassName('NgxEditor__Content')
      contentEditors[0].addEventListener("paste", function(event) {
        event.preventDefault();
        const plainTextClipboard = event.clipboardData.getData('text/plain');
        if (document.queryCommandSupported?.("insertText")) {
          document.execCommand("insertText", false, plainTextClipboard);
        } else {
          document.execCommand("paste", false, plainTextClipboard);
        }
      }, true);
    }
  }

  ngOnInit() {
    this.toolbar = [
      ['bold', 'italic', 'underline'],
      [{ heading: this.hideHeading1 ? ['h2', 'h3', 'h4', 'h5', 'h6'] : ['h1', 'h2', 'h3', 'h4', 'h5', 'h6'] }],
      ['link'],
      ['bullet_list', 'ordered_list'],
    ];
    this.editor = new Editor({
      history: true
    });
    if (this.options.heightInPixel && this.options.heightInPixel > 0) {
      this.minHeight = this.options.heightInPixel.toString() + 'px';
    }
  }

  ngOnDestroy() {
    this.editor.destroy();
  }

  onChange(data: any) {
    this.control.setValue(data);
    this.control.markAsDirty();
    this.control.markAsTouched();
  }

  setTouched() {
    this.control.markAsDirty();
    this.control.markAsTouched();
  }
}
