import React, { useState } from 'react';
import { TodoListResponseModel } from '../../api/client';
import './styles.css';
import { useUpdateTodoList, useDeleteTodoList } from '../../controller/useTodoListApi';

export const TodoListRow: React.FC<{
  list: TodoListResponseModel;
  onListClick: () => void;
  mutate: () => void;
}> = ({
  list,
  onListClick,
  mutate,
}) => {
  const { updateTodoList } = useUpdateTodoList(mutate);
  const { deleteTodoList } = useDeleteTodoList(mutate);

  const [isEditing, setIsEditing] = useState(false);
  const [editedListName, setEditedListName] = useState('');
  const [mutationError, setMutationError] = useState<string | null>(null);

  const handleEditClick = () => {
    setIsEditing(true);
    setEditedListName(list.name ?? '');
  };

  const handleSaveEditClick = async () => {
    try {
      await updateTodoList(list.id!, { name: editedListName });
      setIsEditing(false);
      setEditedListName('');
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to save edit');
    }
  };

  const handleDeleteClick = async () => {
    try {
      await deleteTodoList(list.id!);
      setMutationError(null);
    } catch (err: any) {
      setMutationError(err?.message || 'Failed to delete list');
    }
  };

  const handleCardClick = () => {
    if (!isEditing) {
      onListClick();
    }
  };

  return (
    <div className="todo-list-card" onClick={handleCardClick}>
      {mutationError && <div className="error-message">{mutationError}</div>}
      {isEditing ? (
        <>
          <input
            type="text"
            value={editedListName}
            onChange={(e) => setEditedListName(e.target.value)}
            className="edit-list-input"
          />
          <button
            onClick={(e) => { e.stopPropagation(); handleSaveEditClick(); }}
            className="save-edit-button"
          >
            Save
          </button>
          <button
            onClick={(e) => { e.stopPropagation(); setIsEditing(false); }}
            className="cancel-edit-button"
          >
            Cancel
          </button>
        </>
      ) : (
        <>
          <h2 className="todo-list-card-title">{list.name}</h2>
          <div className="todo-list-card-actions">
            <span
              className="material-icons edit-icon"
              onClick={(e) => { e.stopPropagation(); handleEditClick(); }}
            >
              edit
            </span>
            <span
              className="material-icons delete-icon"
              onClick={(e) => { e.stopPropagation(); handleDeleteClick(); }}
            >
              delete
            </span>
          </div>
        </>
      )}
    </div>
  );
};