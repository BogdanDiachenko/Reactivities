import React from 'react';
import { Modal } from 'semantic-ui-react';
import { useStore } from '../../stores';

export function ModalContainer() {
    const { modalStore } = useStore();

    return (
        <Modal open={modalStore.modal.open} onClose={modalStore.closeModal} size='mini'>
            <Modal.Content> 
                {modalStore.modal.body}
            </Modal.Content>
        </Modal>
    );
}