import { CommunicationLogMessageType } from "./communication-log-message-type";
import { CommunicationLogSendType } from "./communication-log-send-type";
import { CommunicationLogStatus } from "./communication-log-status";

export interface CommunicationLogDetails {
  id: string;
  participantId: string;
  participantName: string;
  messageType: CommunicationLogMessageType;
  sendType: CommunicationLogSendType;
  message: string;
  shortMessage: string;
  status: CommunicationLogStatus;
  error?: string;
  createdAt: Date;
}
