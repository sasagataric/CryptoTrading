import React, { useEffect, useState } from 'react'
import { FormControl, InputGroup } from 'react-bootstrap';
import "./SearchCoin.css";

interface IProps{
  setSearch: React.Dispatch<React.SetStateAction<string>>
  search: string;
}
const SearchCoin: React.FC<IProps> = ({setSearch,search}) => {
    const [searchTerm, setSearchTerm] = useState<string>("");

    const handleChange = (e:any) => {
       setSearchTerm(e.target.value);
    };
    useEffect(() => {
      setSearchTerm(search);
    },[])

    const onKeyUp = (event:any) => {
      if (event.charCode === 13) {
        setSearch(searchTerm);
      }
    }

     return (
       <div >
         <InputGroup className="mb-3">
           {
             search!=="" &&
             <InputGroup.Text 
             className="bg-light-red clickable p-1 "
             onClick={()=>setSearch("")}>
               <i className="text-white bi bi-x-lg mx-2"></i>
             </InputGroup.Text>
           }
          <FormControl
          className="text-center input-focus border border-2 shadow-none"
          placeholder="Search by name or symbol"
          aria-label="Search by name or symbol"
          onKeyPress={onKeyUp}
          value={searchTerm}
          onChange={(e)=>handleChange(e)}
          />
          <InputGroup.Text 
          className="clickable hover-dark-grey p-1"
          onClick={()=>setSearch(searchTerm)}>
            <i className="bi bi-search mx-2"></i>
          </InputGroup.Text>
        </InputGroup>
      </div>
     );
}

export default SearchCoin
