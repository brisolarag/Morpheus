import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { JobService } from '../../core/services/job';
import { Job } from '../../core/models/job.model';
import { CvEditorComponent } from '../../shared/components/cv-editor/cv-editor';
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
  name: 'highlight',
  standalone: true
})
export class HighlightPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }

  transform(text: string | undefined, terms: string[]): SafeHtml {
    if (!text) return '';
    let result = text;
    // Simple mock logic to highlight common keywords if they exist in the text
    terms.forEach(term => {
      const regex = new RegExp(`(${term})`, 'gi');
      result = result.replace(regex, `<span class="bg-yellow-200 text-yellow-900 px-1 rounded-sm font-medium">$1</span>`);
    });
    // Also convert newlines to <br> for HTML rendering
    result = result.replace(/\n/g, '<br/>');
    return this.sanitizer.bypassSecurityTrustHtml(result);
  }
}

@Component({
  selector: 'app-cv-editor-page',
  standalone: true,
  imports: [CommonModule, CvEditorComponent, HighlightPipe],
  templateUrl: './cv-editor-page.html',
  styleUrl: './cv-editor-page.css'
})
export class CvEditorPage implements OnInit {
  private route = inject(ActivatedRoute);
  private jobService = inject(JobService);
  private location = inject(Location);
  private cdr = inject(ChangeDetectorRef);

  job: Job | null = null;
  loading = true;
  errorMessage: string | null = null;

  highlightTerms = ['Node.js', '.NET', 'Angular', 'TypeScript', 'Data Center', 'React', 'JavaScript', 'REST APIs'];

  ngOnInit(): void {
    try {
      const id = this.route.snapshot.paramMap.get('id');
      if (id) {
        this.jobService.getJobById(id).subscribe({
          next: (job) => {
            this.job = job || null;
            if (!this.job) this.errorMessage = "Job returned as null";
            this.loading = false;
            this.cdr.markForCheck();
          },
          error: (err) => {
            console.error(err);
            this.errorMessage = err?.message || 'Error fetching job';
            this.loading = false;
            this.cdr.markForCheck();
          }
        });
      } else {
        this.errorMessage = "No job ID provided in URL";
        this.loading = false;
        this.cdr.markForCheck();
      }
    } catch (err: any) {
      this.errorMessage = "Exception in ngOnInit: " + (err?.message || err);
      this.loading = false;
      this.cdr.markForCheck();
    }
  }

  goBack(): void {
    this.location.back();
  }
}
