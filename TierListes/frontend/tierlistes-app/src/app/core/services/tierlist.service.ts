import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Company, Tier, TierLevel } from '../models/tierlist.models';

@Injectable({
  providedIn: 'root'
})
export class TierlistService {
  private readonly API_URL = 'http://localhost:5000/api/company';
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
    this.http.get<Company[]>(this.API_URL, { withCredentials: true }).subscribe({
      next: (companies) => {
        this.allCompanies = companies;
        this.unrankedCompanies.set([...companies]);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
      }
    });
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
  }
}
