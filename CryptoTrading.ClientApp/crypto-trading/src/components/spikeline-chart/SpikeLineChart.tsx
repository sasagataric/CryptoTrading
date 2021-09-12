import React from 'react'
import { LineChart, Line, YAxis, ResponsiveContainer } from 'recharts';


const SpikeLineChart : React.FC<{ data: number[]}> = ({data}) => {
    const chartColor= (data: number[]) => data[0]>data[data.length - 1]? "#ed4242":"#2bd93a";
    return (
        <ResponsiveContainer width="85%" 
        aspect={2.8} >
        <LineChart  data={data} margin={{right: 0, left: 0}}>
          <Line dataKey={v=>v} stroke={chartColor(data)} strokeWidth={1} dot={false} />
          <YAxis width={0} axisLine={false} tick={false} domain={[data[0], data[data.length - 1]]}/>
        </LineChart>
      </ResponsiveContainer>
    )
}

export default SpikeLineChart
