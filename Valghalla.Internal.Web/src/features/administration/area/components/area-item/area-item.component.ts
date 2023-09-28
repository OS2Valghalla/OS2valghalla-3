import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { AreaHttpService } from '../../services/area-http.service';
import { FormBuilder, Validators } from '@angular/forms';
import { UpdateAreaRequest } from '../../models/update-area-request';
import { CreateAreaRequest } from '../../models/create-area-request';
import { FormPageComponent } from 'src/shared/components/form-page/form-page.component';
import { FormPageEvent } from 'src/shared/models/ux/form-page';
import { AreaDetails } from '../../models/area-details';

@Component({
  selector: 'app-admin-area-item',
  templateUrl: './area-item.component.html',
  providers: [AreaHttpService],
})
export class AreaItemComponent implements AfterViewInit, OnDestroy {
  private readonly subs = new SubSink();

  loading: boolean = true;
  item?: AreaDetails;

  readonly form = this.formBuilder.group({
    name: ['', Validators.required],
    description: [undefined],
  });

  @ViewChild(FormPageComponent) private readonly formPage: FormPageComponent;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly areaHttpService: AreaHttpService,
  ) {}

  ngAfterViewInit(): void {
    this.subs.sink = this.formPage.state$.subscribe(() => {
      if (!this.formPage.isUpdateForm()) {
        this.loading = false;
        this.changeDetectorRef.detectChanges();
        return;
      }

      this.subs.sink = this.areaHttpService
        .getAreaDetails(this.formPage.itemId)
        .subscribe((res) => {
          this.loading = false;
          this.item = res.data;

          if (res.data) {
            this.form.setValue({
              name: res.data.name,
              description: res.data.description,
            });
          }
        });
    });
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  createArea(event: FormPageEvent) {
    const request: CreateAreaRequest = {
      name: this.form.value.name,
      description: this.form.value.description,
    };

    this.subs.sink = event.pipe(this.areaHttpService.createArea(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  updateArea(event: FormPageEvent) {
    const request: UpdateAreaRequest = {
      id: this.formPage.itemId,
      name: this.form.value.name,
      description: this.form.value.description,
    };

    this.subs.sink = event.pipe(this.areaHttpService.updateArea(request)).subscribe((res) => {
      if (res.isSuccess) {
        this.formPage.navigate();
      }
    });
  }

  deleteArea(event: FormPageEvent) {
    this.subs.sink = event
      .pipe(this.areaHttpService.deleteArea(this.formPage.itemId))
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
