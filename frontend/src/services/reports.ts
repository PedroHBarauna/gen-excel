import { api } from "./api";

export type Category = { id: string; name: string };
export type Field = { id: string; label: string };

export type SearchResult = { id: string; text: string };

export type TextDetails = {
  id: string;
  text: string;
  category: Category;
  fields: Field[];
};

export async function getAllCategories(): Promise<Category[]> {
  const { data } = await api.get<Category[]>("/api/reports/categories");
  return data;
}

export async function searchTexts(query: string): Promise<SearchResult[]> {
  const { data } = await api.get<SearchResult[]>("/api/reports/texts/search", {
    params: { q: query, take: 10 },
  });
  return data;
}

export async function getTextDetails(id: string): Promise<TextDetails> {
  const { data } = await api.get<TextDetails>(`/api/reports/texts/${id}`);
  return data;
}

export type ExportReportItem = {
  eventId: number;
  columns: string[];
  headers: string[];
  includeTotal: boolean;
};

export type ExportSpreadsheetRequest = {
  reportTitle: string;
  reports: ExportReportItem[];
};

export async function exportSpreadsheet(
  body: ExportSpreadsheetRequest,
): Promise<Blob> {
  const res = await api.post("/api/reports/spreadsheet", body, {
    responseType: "blob",
  });

  return res.data as Blob;
}
