import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthHttpInterceptor } from './auth-http.interceptor';
import { AuthService } from './auth.service';

function initializerFactory(authService: AuthService): () => Observable<void> {
  return () => authService.ping();
}

@NgModule({
  imports: [],
  providers: [
    AuthService,
    {
      provide: APP_INITIALIZER,
      useFactory: initializerFactory,
      deps: [AuthService],
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
  ],
})
export class AuthModule {}
