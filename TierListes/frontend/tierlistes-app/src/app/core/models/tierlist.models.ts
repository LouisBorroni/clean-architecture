export interface Company {
  id: string;
  name: string;
  logoUrl: string;
}

export type TierLevel = 'S' | 'A' | 'B' | 'C' | 'D';

export interface Tier {
  level: TierLevel;
  label: string;
  color: string;
  companies: Company[];
}
