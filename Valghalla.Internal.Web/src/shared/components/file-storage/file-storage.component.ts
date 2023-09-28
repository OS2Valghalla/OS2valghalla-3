import { Component, Input, OnDestroy } from '@angular/core';
import { FileSystemFileEntry, NgxFileDropEntry } from 'ngx-file-drop';
import { Observable, catchError, forkJoin, map, of, tap } from 'rxjs';
import { TranslocoService } from '@ngneat/transloco';
import { FileReference } from 'src/shared/models/file-storage/file-reference';
import { FileStorageHttpService } from 'src/shared/services/file-storage-http.service';
import { NotificationService } from 'src/shared/services/notification.service';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-file-storage',
  templateUrl: './file-storage.component.html',
  providers: [],
})
export class FileStorageComponent implements OnDestroy {
  private readonly subs = new SubSink();

  @Input() model: FileReference[];
  @Input() type: string;
  @Input() extensions: string[];
  @Input() multiple: boolean;
  @Input() helpText: string;

  filesToUpload: FileSystemFileEntry[] = [];

  private filesToDelete: FileReference[] = [];

  constructor(private readonly fileStorageHttpService: FileStorageHttpService, 
    private readonly notificationService: NotificationService, 
    private readonly translocoService: TranslocoService,) {}

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  submit() {
    return new Observable<string[]>((subcriber) => {
      this.subs.sink = forkJoin(
        this.filesToDelete.length == 0
          ? [this.generateDummyObservable()]
          : this.filesToDelete.map((i) => this.fileStorageHttpService.deleteFile(i.id)),
      ).subscribe(() => {
        this.filesToDelete = [];
        this.subs.sink = forkJoin(
          this.filesToUpload.length == 0
            ? [this.generateDummyObservable()]
            : this.filesToUpload.map(
                (i) =>
                  new Observable<string>((subcriber) => {
                    this.subs.sink = i.file((file) =>
                      this.fileStorageHttpService.uploadFile(file, i.name, this.type).subscribe((res) => {
                        if (res.isSuccess) {
                          subcriber.next(res.data);
                        } else {
                          subcriber.error();
                        }

                        subcriber.complete();
                      }),
                    );
                  }),
              ),
        )
          .pipe(
            map((fileRefIds) => fileRefIds.filter((id) => typeof id !== 'undefined' || id != null)),
            catchError(() => {
              subcriber.complete();
              return of();
            }),
          )
          .subscribe((fileRefIds) => {
            this.filesToUpload = [];
            const relevantFileRefIds = this.model ? this.model.map((i) => i.id) : [];
            relevantFileRefIds.push(...(fileRefIds as any));

            subcriber.next(relevantFileRefIds);
            subcriber.complete();
          });
      });
    });
  }

  deleteAllFiles() {
    return new Observable<void>((subcriber) => {
      this.subs.sink = forkJoin(
        this.filesToDelete.length == 0
          ? [this.generateDummyObservable()]
          : this.filesToDelete.map((i) => this.fileStorageHttpService.deleteFile(i.id)),
      ).subscribe(() => {
        this.filesToDelete = [];
        subcriber.next();
        subcriber.complete();
      });
    });
  }

  onDropped(dropEntries: NgxFileDropEntry[]) {
    if (dropEntries && dropEntries.length == 0) return;

    const invalidFileEntries = dropEntries
      .filter((i) => i.fileEntry.isFile && !this.extensions.some((ext) => i.fileEntry.name.endsWith('.' + ext)))
      .map((i) => i.fileEntry as FileSystemFileEntry);

    if (invalidFileEntries && invalidFileEntries.length > 0) {
      this.notificationService.showWarning(this.translocoService.translate('shared.error.invalid_file_extension'), this.translocoService.translate('shared.error.invalid_file_extension_details') + this.extensions.join(' ,'));
    }

    const validFileEntries = dropEntries
      .filter((i) => i.fileEntry.isFile && this.extensions.some((ext) => i.fileEntry.name.endsWith('.' + ext)))
      .map((i) => i.fileEntry as FileSystemFileEntry);

    if (!this.multiple && validFileEntries && validFileEntries.length > 0){
      this.filesToUpload = [];
      this.model = [];
    }
    this.filesToUpload.push(...validFileEntries);
  }

  removeUploadFile(idx: number) {
    this.filesToUpload = this.filesToUpload.filter((i, index) => index != idx);
  }

  removeUploadedFile(file: FileReference) {
    this.filesToDelete.push(file);
    this.model = this.model.filter((i) => i.id != file.id);
  }

  getFileDownloadLink(file: FileReference) {
    return this.fileStorageHttpService.getDownloadFileLink(file.id);
  }

  private generateDummyObservable() {
    return new Observable<void>((subcriber) => {
      subcriber.next();
      subcriber.complete();
    });
  }
}
