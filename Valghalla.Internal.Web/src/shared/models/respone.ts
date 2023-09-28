export interface ResponseBase {
  isSuccess: boolean;
  confirmation: Confirmation;
  message?: string;
  errors?: ResponseErrorDictionary;
}

export interface ResponseErrorDictionary {
  validation?: string[];
  details?: string[];
}

export interface Response<T> extends ResponseBase {
  data?: T;
}

export interface Confirmation {
  title?: string;
  messages?: string[];
}
