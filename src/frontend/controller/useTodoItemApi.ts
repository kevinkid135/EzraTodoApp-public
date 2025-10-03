import useSWR from 'swr';
import { API_BASE_URL } from '../api/apiConfig';
import {
  Client,
  CreateTodoItemRequestModel,
  UpdateTodoItemRequestModel,
  TodoItemCollectionResponseModel
} from '../api/client';

const client = new Client(API_BASE_URL);

// Fetch hook
export const useFetchTodoItems = (todolistId: number) => {
  const fetcher = (): Promise<TodoItemCollectionResponseModel> =>
    client.todoitems(todolistId, false, undefined);
  const { data: todoItems, error: fetchError, isLoading, mutate } = useSWR<TodoItemCollectionResponseModel>(`todoitems-${todolistId}`, fetcher);

  return {
    data: todoItems,
    isLoading,
    isError: !!fetchError,
    error: fetchError,
    mutate
  };
};

// Create hook
export const useCreateTodoItem = (todolistId: number, mutate: () => void) => {
  const createTodoItem = async (itemData: Partial<CreateTodoItemRequestModel>) => {
    if (!itemData.title) {
      throw new Error("Title is required to create a todo item.");
    }
    const payload = new CreateTodoItemRequestModel({
      ...itemData,
      title: itemData.title
    });
    await client.todoitemPOST(todolistId, payload);
    mutate();
  };
  return { createTodoItem };
};

// Update hook
export const useUpdateTodoItem = (todolistId: number, mutate: () => void) => {
  const updateTodoItem = async (todoitemId: number, updatedData: Partial<UpdateTodoItemRequestModel>) => {
    const payload = new UpdateTodoItemRequestModel(updatedData);
    await client.todoitemPUT(todolistId, todoitemId, payload);
    mutate();
  };
  return { updateTodoItem };
};

// Delete hook
export const useDeleteTodoItem = (todolistId: number, mutate: () => void) => {
  const deleteTodoItem = async (todoitemId: number) => {
    const payload = new UpdateTodoItemRequestModel({ isDeleted: true });
    await client.todoitemPUT(todolistId, todoitemId, payload);
    mutate();
  };
  return { deleteTodoItem };
};