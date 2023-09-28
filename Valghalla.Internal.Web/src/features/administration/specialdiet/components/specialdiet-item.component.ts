import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { SpecialDiet } from '../models/specialdiet';
import { CreateSpecialDietRequest } from '../models/create-specialdiet-request';
import { UpdateSpecialDietRequest } from '../models/update-specialdiet-request';
import { SpecialDietHttpService } from '../services/specialdiet-http.service';
import { SubSink } from 'subsink';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';

@Component({
  selector: 'app-admin-specialdiet-item',
  templateUrl: './specialdiet-item.component.html',
  providers: [SpecialDietHttpService],
})
export class SpecialDietItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading = true;
  item: SpecialDiet;

  readonly form = this.formBuilder.group({
    title: ['', Validators.required],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly specialdietHttpService: SpecialDietHttpService,
  ) {}

  ngAfterViewInit(): void {
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.specialdietHttpService.getSpecialDiet(this.formPage.itemId).subscribe((res) => {
        this.loading = false;
        this.item = res.data;

        if (res.data) {
          this.form.setValue({
            title: res.data.title,
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createSpecialDiet(event: FormPageEvent) {
    const createRequest: CreateSpecialDietRequest = {
      title: this.form.value.title,
    };

    this.subs.sink = event.pipe(this.specialdietHttpService.createSpecialDiet(createRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateSpecialDiet(event: FormPageEvent) {
    const updateRequest: UpdateSpecialDietRequest = {
      id: this.formPage.itemId,
      title: this.form.value.title,
    };

    this.subs.sink = event.pipe(this.specialdietHttpService.updateSpecialDiet(updateRequest)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteSpecialDiet(event: FormPageEvent) {
    this.subs.sink = event
      .pipe(this.specialdietHttpService.deleteSpecialDiet(this.formPage.itemId))
      .subscribe((res) => {
        if (res.isSuccess) {
          this.formPage.navigate();
        }
      });
  }

  cancel() {
    this.formPage.navigate();
  }
}
