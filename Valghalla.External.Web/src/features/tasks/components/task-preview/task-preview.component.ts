import { Component, Input } from '@angular/core';
import { TaskPreview } from '../../models/task-preview';

@Component({
  selector: 'app-task-preview',
  templateUrl: './task-preview.component.html',
})
export class TaskPreviewComponent {
  @Input() model: TaskPreview;
}
