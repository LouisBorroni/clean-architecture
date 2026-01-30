import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable } from 'rxjs';
import { Company, Tier, TierLevel } from '../models/tierlist.models';

interface TierListDto {
  companyId: string;
  tierLevel: string;
}

interface TierListItemDto {
  companyId: string;
  tierLevel: string;
}

export interface ExportResponse {
  pdfUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class TierlistService {
  private readonly COMPANY_API_URL = 'http://localhost:5000/api/company';
  private readonly TIERLIST_API_URL = 'http://localhost:5000/api/tierlist';
  private http = inject(HttpClient);

  private allCompanies: Company[] = [];

  tiers = signal<Tier[]>([
    { level: 'S', label: 'S', color: '#ff7f7f', companies: [] },
    { level: 'A', label: 'A', color: '#ffbf7f', companies: [] },
    { level: 'B', label: 'B', color: '#ffdf7f', companies: [] },
    { level: 'C', label: 'C', color: '#ffff7f', companies: [] },
    { level: 'D', label: 'D', color: '#bfff7f', companies: [] }
  ]);

  unrankedCompanies = signal<Company[]>([]);
  isLoading = signal(false);

  loadCompanies(): void {
    this.isLoading.set(true);

    forkJoin({
      companies: this.http.get<Company[]>(this.COMPANY_API_URL, { withCredentials: true }),
      rankings: this.http.get<TierListDto[]>(this.TIERLIST_API_URL, { withCredentials: true })
    }).subscribe({
      next: ({ companies, rankings }) => {
        this.allCompanies = companies;
        this.restoreState(companies, rankings);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
  }

  private restoreState(companies: Company[], rankings: TierListDto[]): void {
    const rankingMap = new Map(rankings.map(r => [r.companyId, r.tierLevel]));

    const tiers = this.tiers();
    tiers.forEach(tier => tier.companies = []);

    const unranked: Company[] = [];

    companies.forEach(company => {
      const tierLevel = rankingMap.get(company.id);

      if (tierLevel && tierLevel !== 'unranked') {
        const tier = tiers.find(t => t.level === tierLevel);
        if (tier) {
          tier.companies.push(company);
        } else {
          unranked.push(company);
        }
      } else {
        unranked.push(company);
      }
    });

    this.tiers.set([...tiers]);
    this.unrankedCompanies.set(unranked);
  }

  saveState(): void {
    const rankings: TierListItemDto[] = [];

    this.tiers().forEach(tier => {
      tier.companies.forEach(company => {
        rankings.push({
          companyId: company.id,
          tierLevel: tier.level
        });
      });
    });

    this.http.post(this.TIERLIST_API_URL, { rankings }, { withCredentials: true }).subscribe();
  }

  moveToTier(company: Company, tierLevel: TierLevel): void {
    this.removeFromCurrentPosition(company);

    const tiers = this.tiers();
    const tier = tiers.find(t => t.level === tierLevel);
    if (tier) {
      tier.companies.push(company);
      this.tiers.set([...tiers]);
    }
  }

  moveToUnranked(company: Company): void {
    this.removeFromCurrentPosition(company);
    this.unrankedCompanies.update(companies => [...companies, company]);
  }

  private removeFromCurrentPosition(company: Company): void {
    this.unrankedCompanies.update(companies =>
      companies.filter(c => c.id !== company.id)
    );

    const tiers = this.tiers();
    tiers.forEach(tier => {
      tier.companies = tier.companies.filter(c => c.id !== company.id);
    });
    this.tiers.set([...tiers]);
  }

  reorderInTier(tierLevel: TierLevel, previousIndex: number, currentIndex: number): void {
    const tiers = this.tiers();
    const tier = tiers.find(t => t.level === tierLevel);
    if (tier) {
      const item = tier.companies.splice(previousIndex, 1)[0];
      tier.companies.splice(currentIndex, 0, item);
      this.tiers.set([...tiers]);
    }
  }

  reset(): void {
    this.tiers.set([
      { level: 'S', label: 'S', color: '#ff7f7f', companies: [] },
      { level: 'A', label: 'A', color: '#ffbf7f', companies: [] },
      { level: 'B', label: 'B', color: '#ffdf7f', companies: [] },
      { level: 'C', label: 'C', color: '#ffff7f', companies: [] },
      { level: 'D', label: 'D', color: '#bfff7f', companies: [] }
    ]);
    this.unrankedCompanies.set([...this.allCompanies]);
    this.saveState();
  }

  exportPdf(imageBase64: string): Observable<ExportResponse> {
    return this.http.post<ExportResponse>(
      `${this.TIERLIST_API_URL}/export`,
      { imageBase64 },
      { withCredentials: true }
    );
  }
}
