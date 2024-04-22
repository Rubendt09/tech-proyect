export * from "./statuses";
export * from "./PostCard";
export * from "./PostColumn";
export * from "./PostList";
export * from "./PostListContent";

export interface Post {
  id: string;
  title: string;
  code: string,
  content: string;
  status: "to_do" | "in_process" | "done" ;
  index: number;
}


export interface Order {
  id: string;
  name: string;
  code: string;
  typeOrder: "armed" | "other_type"; // Añade otros tipos si los hay
  phase: "to_do" | "in_process" | "done"; // Asumo que puede haber otras fases similares a la interfaz Post
  stopping: boolean;
  optionDowntime: string;
  reprogramate: boolean;
  reason: string;
}

