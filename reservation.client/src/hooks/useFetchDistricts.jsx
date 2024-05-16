import axios from 'axios'
import React, { useEffect, useState } from 'react'

const API_URL = "https://vapi.vnappmob.com/api/province/district/"

export default function useFetchDistricts(province_id) {
    const [districts, setDistricts] = useState([])
    
    useEffect(() => {
        const fetchDistricts = async () => {
            const response = await axios.get(API_URL + province_id)
            const results = response.data.results.map(item => ({
                label: item.district_name,
                value: item.district_name
            }))

            setDistricts(results)
        }

        fetchDistricts()
    }, [province_id])

    return districts;
}
