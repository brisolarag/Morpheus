import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs';

export interface CvModel {
  id: string;
  name: string;
  importedAt: Date;
}

@Injectable({
  providedIn: 'root'
})
export class CvService {
  private mockCvs: CvModel[] = [
    { id: '1', name: 'Software_Engineer_CV.pdf', importedAt: new Date('2026-05-10T10:00:00Z') },
    { id: '2', name: 'Resume_JohnSmith_2025.pdf', importedAt: new Date('2025-11-20T14:30:00Z') },
    { id: '3', name: 'Draft_CV_old.pdf', importedAt: new Date('2024-08-05T09:15:00Z') },
  ];

  getRecentCvs(): Observable<CvModel[]> {
    return of(this.mockCvs).pipe(delay(500));
  }
}
