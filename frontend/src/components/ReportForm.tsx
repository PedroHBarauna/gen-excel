import { useState, useEffect } from "react";
import { Plus, Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import AutocompleteInput, { AutocompleteOption } from "./AutocompleteInput";
import { useDebounce } from "@/hooks/useDebounce";
import {
  searchTexts,
  getTextDetails,
  getAllCategories,
  type Category,
  type Field,
  type SearchResult,
} from "@/services/reports";

export interface ReportItem {
  id: string;
  eventId: number;
  text: string;
  category: string;
  fields: string[];
  headers: string[];
  includeTotal: boolean;
}

interface ReportFormProps {
  onAdd: (item: ReportItem) => void;
}

const ReportForm = ({ onAdd }: ReportFormProps) => {
  const [text, setText] = useState("");
  const [selectedTextId, setSelectedTextId] = useState<string | null>(null);
  const [category, setCategory] = useState("");
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [availableFields, setAvailableFields] = useState<Field[]>([]);

  const [searchResults, setSearchResults] = useState<AutocompleteOption[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [isSearching, setIsSearching] = useState(false);
  const [isLoadingDetails, setIsLoadingDetails] = useState(false);

  const debouncedText = useDebounce(text, 400);

  useEffect(() => {
    (async () => {
      try {
        const data = await getAllCategories();
        setCategories(Array.isArray(data) ? data : []);
      } catch {
        setCategories([]);
      }
    })();
  }, []);

  useEffect(() => {
    if (!debouncedText.trim()) {
      setSearchResults([]);
      return;
    }

    if (selectedTextId) return;

    const fetchResults = async () => {
      setIsSearching(true);
      try {
        const results = await searchTexts(debouncedText);
        setSearchResults(
          results.map((r: SearchResult) => ({ id: r.id, label: r.text })),
        );
      } catch (error) {
        setSearchResults([]);
      } finally {
        setIsSearching(false);
      }
    };

    fetchResults();
  }, [debouncedText, selectedTextId]);

  const handleSelectText = async (option: AutocompleteOption) => {
    setSelectedTextId(option.id);
    setSearchResults([]);

    setIsLoadingDetails(true);
    try {
      const details = await getTextDetails(option.id);
      if (details.category) {
        setCategory(details.category.id);
      }
      setAvailableFields(details.fields);
      setSelectedFields([]);
    } catch (error) {
      console.error("Erro ao buscar detalhes:", error);
    } finally {
      setIsLoadingDetails(false);
    }
  };

  const handleTextChange = (value: string) => {
    setText(value);
    if (selectedTextId) {
      setSelectedTextId(null);
      setCategory("");
      setAvailableFields([]);
      setSelectedFields([]);
    }
  };

  const handleFieldToggle = (fieldId: string) => {
    setSelectedFields((prev) =>
      prev.includes(fieldId)
        ? prev.filter((id) => id !== fieldId)
        : [...prev, fieldId],
    );
  };

  const handleSubmit = () => {
    if (
      !text.trim() ||
      !category ||
      selectedFields.length === 0 ||
      !selectedTextId
    )
      return;

    const categoryName =
      categories.find((c) => c.id === category)?.name || category;

    const headers = selectedFields.map(
      (fid) => availableFields.find((f) => f.id === fid)?.label ?? fid,
    );

    onAdd({
      id: crypto.randomUUID(),
      eventId: Number(selectedTextId),
      text: text.trim(),
      category: categoryName,
      fields: selectedFields,
      headers,
      includeTotal: true,
    });

    setText("");
    setSelectedTextId(null);
    setCategory("");
    setSelectedFields([]);
    setSearchResults([]);
  };

  return (
    <div className="rounded-xl border border-border bg-card p-6 shadow-card">
      <div className="flex flex-wrap items-end gap-4">
        <div className="flex-1 min-w-[200px]">
          <Label htmlFor="texto" className="mb-2 block text-sm font-medium">
            Evento
          </Label>
          <AutocompleteInput
            id="texto"
            value={text}
            onChange={handleTextChange}
            onSelect={handleSelectText}
            options={searchResults}
            isLoading={isSearching}
            placeholder="Digite para buscar..."
          />
        </div>

        <div className="w-[180px]">
          <Label htmlFor="categoria" className="mb-2 block text-sm font-medium">
            Categoria
          </Label>
          <div className="relative">
            <Select value={category} onValueChange={setCategory}>
              <SelectTrigger id="categoria" className="bg-background">
                <SelectValue placeholder="Selecione..." />
              </SelectTrigger>
              <SelectContent>
                {categories.map((cat) => (
                  <SelectItem key={cat.id} value={cat.id}>
                    {cat.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
            {isLoadingDetails && (
              <div className="absolute inset-0 flex items-center justify-center bg-background/80 rounded-md">
                <Loader2 className="h-4 w-4 animate-spin text-primary" />
              </div>
            )}
          </div>
        </div>

        <div className="flex flex-wrap items-center gap-4 min-w-[200px]">
          {isLoadingDetails ? (
            <div className="flex items-center gap-2 text-muted-foreground">
              <Loader2 className="h-4 w-4 animate-spin" />
              <span className="text-sm">Carregando campos...</span>
            </div>
          ) : availableFields.length === 0 ? (
            <span className="text-sm text-muted-foreground">
              Selecione um texto para ver os campos
            </span>
          ) : (
            availableFields.map((field) => (
              <div key={field.id} className="flex items-center gap-2">
                <Checkbox
                  id={field.id}
                  checked={selectedFields.includes(field.id)}
                  onCheckedChange={() => handleFieldToggle(field.id)}
                />
                <Label
                  htmlFor={field.id}
                  className="text-sm font-medium cursor-pointer"
                >
                  {field.label}
                </Label>
              </div>
            ))
          )}
        </div>

        <Button onClick={handleSubmit} className="gap-2">
          <Plus className="h-4 w-4" />
          Adicionar
        </Button>
      </div>
    </div>
  );
};

export default ReportForm;
