import { useState } from "react";
import Header from "@/components/Header";
import ReportForm, { ReportItem } from "@/components/ReportForm";
import ReportList from "@/components/ReportList";
import { toast } from "sonner";
import { exportSpreadsheet } from "@/services/reports";

const Index = () => {
  const [items, setItems] = useState<ReportItem[]>([]);

  const handleAdd = (item: ReportItem) => {
    setItems((prev) => [...prev, item]);
    toast.success("Item adicionado com sucesso.");
  };

  const handleRemove = (id: string) => {
    setItems((prev) => prev.filter((item) => item.id !== id));
    toast.info("Item removido.");
  };

  const handleExport = async () => {
    if (items.length === 0) {
      toast.error("Adicione pelo menos um item.");
      return;
    }

    try {
      const payload = {
        reportTitle: "Relatório de Eventos",
        reports: items.map((it) => ({
          eventId: it.eventId,
          columns: it.fields,
          headers: it.headers,
          includeTotal: it.includeTotal,
        })),
      };

      const blob = await exportSpreadsheet(payload);

      const url = URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `Relatorio_Teste.xlsx`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      URL.revokeObjectURL(url);

      toast.success("Relatório exportado com sucesso!");
    } catch (e) {
      console.error(e);
      toast.error("Erro ao exportar.");
    }
  };

  const handleToggleIncludeTotal = (id: string, value: boolean) => {
    setItems((prev) =>
      prev.map((it) => (it.id === id ? { ...it, includeTotal: value } : it)),
    );
  };

  return (
    <div className="min-h-screen bg-background">
      <Header />

      <main className="container mx-auto px-6 py-8">
        <div className="mb-8">
          <h1 className="text-2xl font-bold text-foreground mb-2">
            Gerador de Relatórios de Eventos
          </h1>
          <p className="text-muted-foreground">
            Adicione os eventos abaixo e gere seu relatório em Excel.
          </p>
        </div>

        <div className="space-y-6">
          <ReportForm onAdd={handleAdd} />
          <ReportList
            items={items}
            onRemove={handleRemove}
            onExport={handleExport}
            onToggleIncludeTotal={handleToggleIncludeTotal}
          />
        </div>
      </main>
    </div>
  );
};

export default Index;
