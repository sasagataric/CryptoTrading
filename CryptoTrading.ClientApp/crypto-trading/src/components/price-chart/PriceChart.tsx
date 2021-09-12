import React , {useState, useEffect} from 'react'
import {
    ResponsiveContainer,
    AreaChart,
    XAxis,
    YAxis,
    Area,
    Tooltip,
    CartesianGrid
  } from "recharts";
import { format } from "date-fns";
import './PriceChart.css'
import { utils } from '../../utils/Utils';
  
interface IProp {
    data:{
        date : number,
        price : number
    }[],
    selectedDays : string;
}


const PriceChart :React.FC<IProp> = ({data, selectedDays}) => {
 
    const min = Math.min.apply(null, data.map(function(item) {
        return item.price;
      }))
    const max = Math.max.apply(null, data.map(function(item) {
        return item.price;
      }));

    const [dateFormat, setDateFormat] = useState<string>("");
    const [interval, setInterval]  = useState<number>(0);

    useEffect(() => {
        switch(selectedDays){
            case "1":
                setDateFormat("HH:mm");
                setInterval(10);
                break;
            case "7":
                setDateFormat("dd.MMM");
                setInterval(10);
                break;
            case "30":
                setDateFormat("dd.MMM");
                setInterval(15);
                break;
            case "90":
                setDateFormat("dd.MMM");
                setInterval(15);
                break;
            case "365":
                setDateFormat("MMM `yy");
                setInterval(15);
                break;
            case "max":
                setDateFormat("MMM `yy");
                setInterval(15);
                break;
            default:
                setDateFormat("d.MM.y");
                setInterval(10);
        }
    }, [selectedDays])
    
    
    
    
    const chartColor= () => data[0].price>data[data.length - 1].price? "#ed4242":"#2bd93a";
    return (
        <ResponsiveContainer  
        width="95%" 
        aspect={1.9}
        >
        <AreaChart data={data} margin={{ right: 30, left: 30 }}>
          <defs>
            <linearGradient id="color" x1="0" y1="0" x2="0" y2="1">
              <stop offset="0%" stopColor={chartColor()} stopOpacity={0.4} />
              <stop offset="75%" stopColor={chartColor()} stopOpacity={0.05} />
            </linearGradient>
          </defs>
  
          <Area dataKey="price" stroke={chartColor()} strokeWidth={1.5} fill="url(#color)" />
  
          <XAxis
            dataKey="date"
            axisLine={false}
            tickLine={false}
            tickCount={interval}
            dy={5}
            dx={5}
            tick={{fontSize: 13}}
            tickFormatter={(str) => {
                return format(new Date(str), dateFormat);
            }}
            type="number"
            domain={[data[0].date, data[data.length-1].date]}
          />
  
          <YAxis
            dataKey="price"
            axisLine={false}
            tickLine={false}
            tickCount={8}
            dx={-5}
            dy={-5}
            tick={{fontSize: 14}}
            type="number"
            domain={[min - (min/100)*2 ,max + (max/100)*2 ]}
            tickFormatter={(number:number) => {return "€"+number.toFixed(2)}}
          />
  
          <Tooltip content={<CustomTooltip />}/>
  
          <CartesianGrid opacity={0.1} vertical={false} />
        </AreaChart>
      </ResponsiveContainer>
    )
}
const CustomTooltip =({ active, payload, label }:  any) => {
    if (active) {
      return (
        <div className="custom-tooltip bg-white p-2 rounded shadows ">
          <p className="my-1 fw-bold font-09">{format(label,"dd-MM-yyyy HH:ss")}</p>
          <p className="my-1 fw-bold font-09">Price: €{utils.getToLocalString(payload?.[0].value)}</p>
        </div>
      );
    }
    return null;
  }

export default PriceChart
