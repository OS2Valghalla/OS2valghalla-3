import { Component, OnInit } from '@angular/core';
import { SubSink } from 'subsink';
import { FormBuilder } from '@angular/forms';
import { Tabnav } from 'dkfds';
import { TranslocoService } from '@ngneat/transloco';
import { AppWeekdayNamePipe } from 'src/shared/pipes/weekday-name.pipe';
import { AppMonthNamePipe } from 'src/shared/pipes/month-name.pipe';
import { WorkLocationHttpService } from '../services/work-location-http.service';
import { WorkLocationShared } from 'src/shared/models/work-location-shared';
import { WorkLocationDate, WorkLocationParticipantDetails } from '../models/work-location-details';

@Component({
  selector: 'app-my-work-location-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['landing.component.scss'],
  providers: [AppWeekdayNamePipe, AppMonthNamePipe, WorkLocationHttpService]
})
export class MyWorkLocationLandingComponent implements OnInit {
  private readonly subs = new SubSink();

  loading = true;

  loadingWorkLocationDates = false;
  
  itemsPerPage: number = 25;

  currentPage: number = 1;

  pageCount: number = 0;

  workLocations: Array<WorkLocationShared> = [];
  
  workLocationDates: Array<WorkLocationDate> = [];

  displayParticipants: Array<WorkLocationParticipantDetails> = [];

  readonly form = this.formBuilder.group({
    selectedWorkLocationId: [''],
    keyword: ''
  });

  constructor(
    private formBuilder: FormBuilder,
    private readonly appWeekdayNamePipe: AppWeekdayNamePipe,
    private readonly appMonthNamePipe: AppMonthNamePipe,
    private readonly translocoService: TranslocoService,
    private workLocationHttpService: WorkLocationHttpService,
    ) {    
    }

  ngOnInit(): void {
    this.subs.sink = this.workLocationHttpService.getMyTeams().subscribe((res) => {
      if (res.data) {
        this.workLocations = res.data;
        this.form.controls.selectedWorkLocationId.setValue(this.workLocations[0].id);
        this.getWorkLocationDetails();
      }
      
      this.loading = false;
    });    
  }

  onFilterChanged() { 
    this.getWorkLocationDetails();
  }

  getWorkLocationDetails() {
    this.workLocationDates = [];
    this.displayParticipants = [];
    this.loadingWorkLocationDates = true;
    this.subs.sink = this.workLocationHttpService.getWorkLocationDates(this.form.controls.selectedWorkLocationId.value).subscribe((res) => {
      if (res.data) {
        res.data.forEach(taskDate => {
          this.workLocationDates.push({
            taskDate: taskDate
          })
        });
        this.getWorkLocationTaskDateDetails(this.workLocationDates[0]);
      }
      this.loadingWorkLocationDates = false;
      
      setTimeout(() => {
        new Tabnav(document.getElementById('tabsDates')).init();

        const tabnavItemElements = Object.values(document.getElementsByClassName('tabnav-item'));
        tabnavItemElements.forEach(element => {
          element.addEventListener('fds.tabnav.open', (event) => {
            var tabId = (event.target as any).id;
            var workLocationDate = this.workLocationDates.filter(f => 'tab_' + f.taskDate == tabId)[0];
            this.getWorkLocationTaskDateDetails(workLocationDate);
          });
        });
      }, 100);
    });
  }

  getWorkLocationTaskDateDetails(workLocationDate) {
    this.form.controls.keyword.setValue(undefined);
    this.currentPage = 1;
    if (!workLocationDate.detailsLoaded) {
      this.subs.sink = this.workLocationHttpService.getWorkLocationDetails(this.form.controls.selectedWorkLocationId.value, workLocationDate.taskDate).subscribe((res) => {
        workLocationDate.detailsLoaded = true;
        workLocationDate.taskDetails = res.data;
        this.displayParticipants = workLocationDate.taskDetails.participants;
        this.pageCount = Math.ceil(this.displayParticipants.length / this.itemsPerPage);
      });
    } else {
      this.displayParticipants = workLocationDate.taskDetails.participants;
      this.pageCount = Math.ceil(this.displayParticipants.length / this.itemsPerPage);
    }
  }

  sortByTaskName(event, workLocationDate: WorkLocationDate) {
    var ascending = event.target.parentElement.parentElement.getAttribute('aria-sort') == 'ascending';
    workLocationDate.taskDetails.taskTypes.sort((taskType1, taskType2) => {
      return taskType1.title > taskType2.title ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
    });
    
    if (ascending) { 
      event.target.parentElement.parentElement.setAttribute('aria-sort', 'descending');
      event.target.children[0].setAttribute('xlink:href', '#sort-table-descending');
    }
    else {
      event.target.parentElement.parentElement.setAttribute('aria-sort', 'ascending');
      event.target.children[0].setAttribute('xlink:href', '#sort-table-ascending');
    }
  }

  sortParticipants(event, columnName: string, workLocationDate: WorkLocationDate) {
    var ascending = event.target.parentElement.parentElement.getAttribute('aria-sort') == 'ascending';

    const theadElements = Object.values(document.getElementById('participantsTable_' + workLocationDate.taskDate).getElementsByTagName('thead')[0].getElementsByTagName('th'));    
    theadElements.forEach(el => { 
      if ((el as any).getAttribute('sort-by') != columnName) {
        const useElements = Object.values((el as any).getElementsByTagName('use'));
        if (useElements && useElements.length > 0) {
          (el as any).removeAttribute('aria-sort');
          (useElements[0] as any).setAttribute('xlink:href', '#sort-table-none');
        }
      } else {
        const useElements = Object.values((el as any).getElementsByTagName('use'));
        if (useElements && useElements.length > 0) {
          if (ascending) {
            (el as any).setAttribute('aria-sort', 'descending');
            (useElements[0] as any).setAttribute('xlink:href', '#sort-table-descending');
          }
          else {
            (el as any).setAttribute('aria-sort', 'ascending');
            (useElements[0] as any).setAttribute('xlink:href', '#sort-table-ascending');
          }
        }
      }
    });

    this.displayParticipants.sort((participant1, participant2) => {
      switch (columnName) {
        case 'fullName':
          return participant1.fullName > participant2.fullName ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
        case 'taskTypes':
          return participant1.taskTypes > participant2.taskTypes ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
        case 'teams':
          return participant1.teams > participant2.teams ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
        case 'age':
          return participant1.age > participant2.age ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
        default:
          return participant1.fullName > participant2.fullName ? (ascending ? -1 : 1) : (ascending ? 1 : -1);
      }      
    });
  }

  searchParticipants(workLocationDate: WorkLocationDate) {
    this.displayParticipants = workLocationDate.taskDetails.participants.filter(t => t.fullName.toLowerCase().indexOf(this.form.controls.keyword.value.toLowerCase()) > -1);
    this.pageCount = Math.ceil(this.displayParticipants.length / this.itemsPerPage);
  }

  getTaskDateFriendlyText(taskDate, includeWeekday?: boolean, includeYear?: boolean) {
    var date = new Date(taskDate);
    return (includeWeekday ? this.translocoService.translate(this.appWeekdayNamePipe.transform(date)) + ' ' : '') + date.getDate() + '. ' + this.translocoService.translate(this.appMonthNamePipe.transform(date)) + (includeYear ? ' ' + date.getFullYear() : '');
  }
}
