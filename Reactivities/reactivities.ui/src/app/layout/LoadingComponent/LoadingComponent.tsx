import React from 'react'
import { Button, Dimmer, Loader } from 'semantic-ui-react';

interface LoadingComponentProps {
    inverted?: boolean;
    content?: string;
}

export function LoadingComponent({ inverted = true, content = 'Loading...' }: LoadingComponentProps) {
    return (
        <Dimmer inverted={inverted} active>
            <Loader content={content} active className='workaround' size='large' inline='centered' />
      </Dimmer>
    )
}