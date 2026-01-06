import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { LoadinService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadinService);

  loadingService.loading();

  return next(req).pipe(
    delay(2000), // remove delay on production,
    finalize(() => {
      loadingService.idle()
    })
  );
};