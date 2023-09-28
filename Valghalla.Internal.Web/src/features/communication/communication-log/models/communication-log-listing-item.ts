import { CommunicationLogMessageType } from './communication-log-message-type';
import { CommunicationLogSendType } from './communication-log-send-type';
import { CommunicationLogStatus } from './communication-log-status';

export interface CommunicationLogListingItem {
  id: string;
  participantId: string;
  participantName: string;
  messageType: CommunicationLogMessageType;
  sendType: CommunicationLogSendType;
  shortMessage: string;
  status: CommunicationLogStatus;
  createdAt: Date;
}
