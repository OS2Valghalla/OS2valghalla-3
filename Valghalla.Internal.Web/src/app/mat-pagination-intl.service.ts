import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { TranslocoService } from '@ngneat/transloco';

@Injectable()
export class MatPaginationIntlService extends MatPaginatorIntl {
  constructor(private translocoService: TranslocoService) {
    super();
    this.itemsPerPageLabel = this.translocoService.translate('shared.paginator.items_per_page');
    this.translocoService.selectTranslate('shared.paginator.items_per_page').subscribe((value) => {
      this.itemsPerPageLabel = value;
      this.changes.next();
    });
  }
}
