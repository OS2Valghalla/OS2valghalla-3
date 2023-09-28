import { Component, Input } from '@angular/core';
import { ParticipantHttpService } from '../../../services/participant-http.service';
import { ParticipantDetails } from 'src/features/participant/models/participant-details';

@Component({
  selector: 'app-participant-validation-information',
  templateUrl: './participant-validation-information.component.html',
  providers: [ParticipantHttpService],
})
export class ParticipantValidationInformationComponent {
  @Input() participant: ParticipantDetails;
}
