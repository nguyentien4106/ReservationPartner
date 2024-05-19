import { Image, Spin } from 'antd'
import React, { useContext, useEffect, useState } from 'react'
import { R2 } from '../../../lib/R2'
import { getUserName } from '../../../lib/helper'
import { CollaboratorInforProfile } from '../CollaboratorInfor'
function AvatarImage() {
    const [avatarUrl, setAvatarUrl] = useState('')
    const [loading, setLoading] = useState(true)
    const collaborator = useContext(CollaboratorInforProfile)

    useEffect(() => {
        if (!collaborator) {
            return
        }

        const userName = getUserName(collaborator.email)
        R2.getAvatar(userName).then(res => {
            setAvatarUrl(res.url)
        })
            .finally(() => {
                setLoading(false)
            })
    }, [collaborator])

    return (
        <>
            {
                loading ? <Spin height={250}/> : <Image
                    height={250}
                    preview={false}
                    src={avatarUrl}
                    style={{
                        borderRadius: "50%"
                    }}
                />
            }
        </>
    )
}

export default AvatarImage
