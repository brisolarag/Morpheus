import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Search } from './search';
import { JobService } from '../../core/services/job';
import { of, throwError } from 'rxjs';
import { JobSearchResponse } from '../../core/models/job.model';
import { vi } from 'vitest';

describe('Search Component', () => {
  let component: Search;
  let fixture: ComponentFixture<Search>;
  let mockJobService: any;

  beforeEach(async () => {
    mockJobService = {
      searchJobs: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [Search],
      providers: [
        { provide: JobService, useValue: mockJobService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Search);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('PS1 - should create', () => {
    expect(component).toBeTruthy();
  });

  it('PS2 - should handle successful search', () => {
    const mockResponse: JobSearchResponse = {
      searchQuery: 'Developer',
      resultsCount: 1,
      results: [
        { id: '1', title: 'Developer', company: 'Tech Inc', companyLogo: '', location: '', seniorityLevel: '', contractType: '', externalPlatform: '', applyUrl: '', publishedAt: '', relevanceScore: 90 }
      ]
    };

    mockJobService.searchJobs.mockReturnValue(of(mockResponse));
    
    component.searchQuery = 'Developer';
    component.onSearch();

    expect(component.isSearching).toBe(false);
    expect(component.hasSearched).toBe(true);
    expect(component.response).toEqual(mockResponse);
    expect(mockJobService.searchJobs).toHaveBeenCalledWith('Developer');
  });

  it('NS1 - should handle search error', () => {
    mockJobService.searchJobs.mockReturnValue(throwError(() => new Error('Search failed')));
    
    component.searchQuery = 'Invalid';
    component.onSearch();

    expect(component.isSearching).toBe(false);
    expect(component.response).toBeNull();
  });
});
