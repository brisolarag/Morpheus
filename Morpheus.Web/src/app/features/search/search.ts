import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JobService } from '../../core/services/job';
import { JobSearchResponse } from '../../core/models/job.model';
import { JobCard } from '../../shared/components/job-card/job-card';

@Component({
  selector: 'app-search',
  imports: [CommonModule, FormsModule, JobCard],
  templateUrl: './search.html',
  styleUrl: './search.css',
})
export class Search {
  private jobService = inject(JobService);
  private cdr = inject(ChangeDetectorRef);

  searchQuery = '';
  isSearching = false;
  hasSearched = false;
  response: JobSearchResponse | null = null;

  onSearch(): void {
    if (!this.searchQuery.trim()) return;

    this.isSearching = true;
    this.hasSearched = true;
    this.cdr.markForCheck();

    this.jobService.searchJobs(this.searchQuery).subscribe({
      next: (res) => {
        this.response = res;
        this.isSearching = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.isSearching = false;
        this.cdr.markForCheck();
      }
    });
  }
}
