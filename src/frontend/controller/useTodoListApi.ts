import useSWR from 'swr';
import { API_BASE_URL } from '../api/apiConfig';
import {
  Client,
  CreateTodoListRequestModel,
  UpdateTodoListRequestModel,
  TodoListCollectionResponseModel
} from '../api/client';

const client = new Client(API_BASE_URL);

// Fetch hook
export const useFetchTodoLists = () => {
  const fetcher = (): Promise<TodoListCollectionResponseModel> =>
    client.todolistGET(false);
  const { data: todoLists, error: fetchError, isLoading, mutate } = useSWR<TodoListCollectionResponseModel>('todolist', fetcher);

  return {
    data: todoLists,
    isLoading,
    isError: !!fetchError,
    error: fetchError,
    mutate
  };
};

// Create hook
export const useCreateTodoList = (mutate: () => void) => {
  const createTodoList = async (listData: { name: string }) => {
    const payload = new CreateTodoListRequestModel({ name: listData.name });
    await client.todolistPOST(payload);
    mutate();
  };
  return { createTodoList };
};

// Update hook
export const useUpdateTodoList = (mutate: () => void) => {
  const updateTodoList = async (
    listId: number,
    updatedData: { name?: string; isDeleted?: boolean }
  ) => {
    const payload = new UpdateTodoListRequestModel(updatedData);
    await client.todolistPUT(listId, payload);
    mutate();
  };
  return { updateTodoList };
};

// Delete hook
export const useDeleteTodoList = (mutate: () => void) => {
  const deleteTodoList = async (listId: number) => {
    const payload = new UpdateTodoListRequestModel({ isDeleted: true });
    await client.todolistPUT(listId, payload);
    mutate();
  };
  return { deleteTodoList };
};