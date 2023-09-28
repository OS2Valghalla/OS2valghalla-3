import { Observable, ReplaySubject, Subscription } from 'rxjs';

interface CacheQueryItem<T> {
  subject: ReplaySubject<T>;
  subject$: Observable<T>;
  subscription?: Subscription;
}

export abstract class CacheQuery {
  private static readonly container: Record<string, CacheQueryItem<unknown>> = {};

  protected query<T>(key: string, observable: Observable<T>): Observable<T> {
    if (!CacheQuery.container[key]) {
      const subject = new ReplaySubject<T>(1);

      CacheQuery.container[key] = {
        subject: subject,
        subject$: subject.asObservable(),
      };
    }

    const cacheItem = CacheQuery.container[key];

    if (cacheItem.subscription) {
      return cacheItem.subject$ as Observable<T>;
    }

    return new Observable<T>((subscriber) => {
      if (!cacheItem.subscription) {
        cacheItem.subscription = observable.subscribe((value) => {
          cacheItem.subject.next(value);
        });
      }

      const cacheSubscription = cacheItem.subject$.subscribe((value) => {
        subscriber.next(value as T);
      });

      return () => cacheSubscription.unsubscribe();
    });
  }

  protected invalidate(key: string): void {
    if (CacheQuery.container[key]) {
      CacheQuery.container[key].subscription.unsubscribe();
      CacheQuery.container[key].subscription = null;
    }
  }
}
