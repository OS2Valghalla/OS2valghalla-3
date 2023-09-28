import { Component, EventEmitter, Input, Output, OnInit, ViewEncapsulation } from '@angular/core';
@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class AppPaginationComponent implements OnInit {

    @Input() pageCount: number;

    @Input() currentPage: number;

    @Output() currentPageChange = new EventEmitter<number>();

    constructor(
    ) {}

    ngOnInit(): void {

    }

    goToPage(event, page) {
        this.currentPage = page + 1;
        this.currentPageChange.emit(this.currentPage);
    }

    previous(event) {
        this.currentPage--;
        this.currentPageChange.emit(this.currentPage);
    }

    next(event) {
        this.currentPage++;
        this.currentPageChange.emit(this.currentPage);
    }

    first(event) {
        this.currentPage = 1;
        this.currentPageChange.emit(this.currentPage);
    }

    last(event) {
        this.currentPage = this.pageCount;
        this.currentPageChange.emit(this.currentPage);
    }
}
