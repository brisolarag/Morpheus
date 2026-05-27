import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Job, JobSearchResponse } from '../models/job.model';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private mockData: JobSearchResponse = {
    searchQuery: "vagas microsoft",
    resultsCount: 1,
    results: [
      {
        id: "27c2deb9-147f-4832-ae74-0a00d1dc080d",
        title: "Oportunidades de Jovem Aprendiz Data Center em Campinas",
        company: "Microsoft",
        companyLogo: "https://media.licdn.com/dms/image/v2/D560BAQH32RJQCl3dDQ/company-logo_100_100/B56ZYQ0mrGGoAU-/0/1744038948046/microsoft_logo?e=2147483647&v=beta&t=rr_7_bFRKp6umQxIHErPOZHtR8dMPIYeTjlKFdotJBY",
        location: "Campinas, São Paulo, Brazil",
        seniorityLevel: "Not Applicable",
        contractType: "Full-time",
        externalPlatform: "External",
        applyUrl: "https://apply.careers.microsoft.com/careers/job/1970393556850552?utm_source=linkedin&domain=microsoft.com&src=LinkedIn",
        publishedAt: "2026-05-22T03:00:00Z",
        relevanceScore: 0.6085112675929617
      }
    ]
  };

  searchJobs(prompt: string): Observable<JobSearchResponse> {
    const response = { ...this.mockData, searchQuery: prompt };
    return new Observable<JobSearchResponse>(observer => {
      setTimeout(() => {
        observer.next(response);
        observer.complete();
      }, 800);
    });
  }

  getFavorites(): Observable<JobSearchResponse> {
    return new Observable<JobSearchResponse>(observer => {
      setTimeout(() => {
        observer.next(this.mockData);
        observer.complete();
      }, 500);
    });
  }

  getJobById(id: string | number): Observable<Job | undefined> {
    const job = this.mockData.results.find(j => j.id == id) || this.mockData.results[0];
    return of(job);
  }
}

