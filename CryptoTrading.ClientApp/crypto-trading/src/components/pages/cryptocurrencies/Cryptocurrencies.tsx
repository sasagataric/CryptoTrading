import React,{useState, useEffect} from 'react'
import CoinsTable from './../../coins-table/CoinsTable'
import {Col,Row, Dropdown, Spinner} from 'react-bootstrap'
import {ICoinsMarkets} from './../../../models/CoinGeckoModels'
import {CoinGeckoService} from './../../../services/coinGeckoAPIService'
import {userService} from './../../../services/userService'
import ReactPaginate from 'react-paginate';
import './Cryptocurrencies.css'

const Cryptocurrencies:React.FC = (props:any) => {
    const [coins,setCoins] = useState<ICoinsMarkets[] | null >([]);
    const [loading,setLoading] = useState<boolean>(true);
    const [rowSize,setRowSize] = useState<number>(10);
    const [pageNumber,setPageNumber] = useState<number>(1);
    const [orderBy,setOrderBy] = useState<string>("market_cap_desc");
    const [watchlist, setWatchlist] = useState<ICoinsMarkets[]> ([]);

    const isActiveRowSize = (number : number) => number===rowSize;

    const isActiveOrderBy = (order : string) => order===orderBy;

    const getPageCount = () =>{
        switch (rowSize) {
            case 10:
                return 900;
            case 50:
                return 180;
            case 100:
                return 90;
            default:
                return 90;
        }
    }

    useEffect(() => {
        
        (async () => {
            setLoading(true);
            const coinsData =await CoinGeckoService.getCoinsMarkets("eur",orderBy,rowSize,pageNumber,[]);
            setCoins(coinsData);
            const watchlistData =await userService.getWatchList();
            if(watchlistData){
                setWatchlist(watchlistData); 
            }
            setLoading(false);
        })()
        
    }, [rowSize,orderBy,pageNumber])

    useEffect(() => {
       const interval = setInterval(async () => {
           if(!loading){
            const coinsData =await CoinGeckoService.getCoinsMarkets("eur",orderBy,rowSize,pageNumber,[]);
            setCoins(coinsData);
           }
            
        },15000)

        return () => clearInterval(interval);
    }, [rowSize,orderBy,pageNumber,loading])

    return (
        <React.Fragment>
            <div className="text-centar pb-4">
                <h1>Today's Cryptocurrency Prices</h1>
            </div>
            {
            loading 
            ?<Spinner animation="border" className="spinner-color" />
            :<>
                <Row className="justify-content-center mb-3">
                    <Dropdown className="col-auto">
                        <Dropdown.Toggle variant="" id="dropdown-basic" className="shadow-none">
                            Row size
                        </Dropdown.Toggle>
                        <Dropdown.Menu >
                            <Dropdown.Item  onClick={()=>{setRowSize(10)}} active={isActiveRowSize(10)} >10</Dropdown.Item>
                            <Dropdown.Item onClick={()=>{setRowSize(50)}} active={isActiveRowSize(50)}>50</Dropdown.Item>
                            <Dropdown.Item onClick={()=>{setRowSize(100)}} active={isActiveRowSize(100)}>100</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                    <Dropdown className="col-auto">
                        <Dropdown.Toggle variant="" id="dropdown-basic" className="shadow-none">
                            Order by
                        </Dropdown.Toggle>
                        <Dropdown.Menu >
                            <Dropdown.Item  onClick={()=>{setOrderBy("market_cap_desc")}} active={isActiveOrderBy("market_cap_desc")} >Market cap desc</Dropdown.Item>
                            <Dropdown.Item onClick={()=>{setOrderBy("market_cap_asc")}} active={isActiveOrderBy("market_cap_asc")}>Market cap asc</Dropdown.Item>
                            <Dropdown.Item onClick={()=>{setOrderBy("volume_desc")}} active={isActiveOrderBy("volume_desc")}>Volume desc</Dropdown.Item> 
                            <Dropdown.Item onClick={()=>{setOrderBy("volume_asc")}} active={isActiveOrderBy("volume_asc")}>Volume asc</Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                </Row>
                <Row className="justify-content-center">
                    <Col xs="auto">
                        <CoinsTable watchlistCoins={watchlist} setWatchlist={setWatchlist} isWatchList={false} coins={coins}/>
                    </Col>
                </Row>
                <Row className={"justify-content-center py-4 "}>
                    <Col xs="auto" className="font-sm-09">
                        <ReactPaginate
                        previousLabel={'<'}
                        nextLabel={'>'}
                        breakLabel={'...'}
                        breakClassName={'break-me'}
                        pageCount={getPageCount()}
                        forcePage={pageNumber-1}
                        marginPagesDisplayed={1}
                        pageRangeDisplayed={4}
                        onPageChange={(data:any)=>setPageNumber(data.selected+1)}
                        containerClassName={'pagination'}
                        pageClassName={' mx-1 px-2 py-1 my-auto rounded'}
                        pageLinkClassName={'fw-bold rounded text-dark'}
                        activeClassName={'active border rounded bg-primary'}
                        activeLinkClassName={'text-white '}
                        previousClassName={'fs-5 mx-1 px-2  border rounded fw-bold text-dark'}
                        nextClassName={'fs-5 mx-1 px-2 border rounded fw-bold text-dark'}
                        />
                    </Col>
                </Row>
            </>
            }
            
            
            
        </React.Fragment>
    )
}

export default Cryptocurrencies
