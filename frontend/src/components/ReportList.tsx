import { Trash2, FileDown } from "lucide-react";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Badge } from "@/components/ui/badge";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import type { ReportItem } from "./ReportForm";

interface ReportListProps {
  items: ReportItem[];
  onRemove: (id: string) => void;
  onExport: () => void;
  onToggleIncludeTotal: (id: string, value: boolean) => void;
}

const ReportList = ({
  items,
  onRemove,
  onExport,
  onToggleIncludeTotal,
}: ReportListProps) => {
  return (
    <div className="flex flex-col rounded-xl border border-border bg-card shadow-card">
      <ScrollArea className="h-[320px] p-4">
        {items.length === 0 ? (
          <div className="flex h-full items-center justify-center text-muted-foreground">
            Nenhum item adicionado ainda.
          </div>
        ) : (
          <div className="space-y-3">
            {items.map((item) => (
              <div
                key={item.id}
                className="flex items-center gap-4 rounded-lg border border-border bg-background p-4 shadow-item transition-all hover:shadow-card"
              >
                <div className="flex-1 min-w-0">
                  <p className="font-medium text-foreground truncate">
                    {item.text}
                  </p>
                </div>

                <Badge variant="secondary" className="shrink-0">
                  {item.category}
                </Badge>

                <div className="flex flex-wrap gap-1.5 shrink-0">
                  {item.fields.map((field) => (
                    <Badge
                      key={field}
                      variant="outline"
                      className="text-xs capitalize"
                    >
                      {field}
                    </Badge>
                  ))}
                </div>

                <div className="flex items-center gap-3 shrink-0">
                  <div className="flex items-center gap-2">
                    <Checkbox
                      id={`total-${item.id}`}
                      checked={!!item.includeTotal}
                      onCheckedChange={(v) =>
                        onToggleIncludeTotal(item.id, v === true)
                      }
                    />
                    <Label
                      htmlFor={`total-${item.id}`}
                      className="text-sm font-medium cursor-pointer select-none"
                    >
                      Total
                    </Label>
                  </div>

                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => onRemove(item.id)}
                    className="text-destructive hover:text-destructive hover:bg-destructive/10"
                  >
                    <Trash2 className="h-4 w-4 mr-1" />
                    Apagar
                  </Button>
                </div>
              </div>
            ))}
          </div>
        )}
      </ScrollArea>

      <div className="border-t border-border p-4 flex justify-end">
        <Button
          onClick={onExport}
          disabled={items.length === 0}
          className="gap-2"
        >
          <FileDown className="h-4 w-4" />
          Gerar Excel
        </Button>
      </div>
    </div>
  );
};

export default ReportList;
