export interface ApiResponse<T> {
    page: number;
    pageSize: number;
    total: number;
    data: T[];

  success: boolean;
  message: string;
  errors: any[];
}
