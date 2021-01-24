import React, { Component } from "react";
import { MDBDataTableV5 } from 'mdbreact';
import './AddOrder2.css'
import Sidebar from '../Sidebar';

export class AddOrderTwo extends Component {
    displayName = AddOrderTwo;
    constructor(props) {
        super(props);        
        this.state = {
            table: {
                columns: [
                    {
                        label: 'Długość',
                        field: 'length',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Szerokość',
                        field: 'width',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Grubość',
                        field: 'thickness',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Kolor',
                        field: 'color',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Rodzaj',
                        field: 'type',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Ilość',
                        field: 'amount',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Usuń',
                        field: 'action',
                        sort: 'asc',
                        width: 150
                    }
                ],
                rows: []
            },
            colors: [],
            type: [],
            check:'',
        }
    }


    componentDidMount() {
        var table2 = [];
        var table3 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i],
                    })
                };
                this.setState({
                    colors: table2

                });
            })

        fetch(`api/Magazine/Return_All_Type`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table3.push({
                        type: json[i],
                    })
                };
                this.setState({
                    type: table3

                });
            })
    }

    handleAddOrder = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                owner: sessionStorage.getItem('client'),
                priority: sessionStorage.getItem('prioryty'),
                deadline: sessionStorage.getItem('deadline'),
            },
            items: this.state.table.rows,
            user: {
                login: sessionStorage.getItem('login'),
            }
        }
        if (this.state.check === '') {
            alert('Nie dodano żadnego elementu')
        }
        else { 
        fetch(`api/Order/Add_Order`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {

                console.log(json)
                return (json);
                this.props.history.push('/orderwarehouse')
            })
            .then(json => {
                this.props.history.push('/orderwarehouse')
            })
    }
    }

    goBack = (event) => {
        sessionStorage.removeItem('client'),
        sessionStorage.removeItem('prioryty'),
        sessionStorage.removeItem('deadline'),
        this.props.history.push('/addorderfirst')
    }

    cancelAdding = (event) => {
        sessionStorage.removeItem('client'),
        sessionStorage.removeItem('prioryty'),
        sessionStorage.removeItem('deadline'),
        this.props.history.push('/orderwarehouse')

    }

    addItem = (event, table) => {
        event.preventDefault();
        var table2 = [];
        if (this.refs.width.value === "" || this.refs.length.value === "" || this.refs.thickness.value === "" || this.refs.color.value === "" || this.refs.amount.value === "" || this.refs.type.value === "" ) {
            alert("Wprowadź dane")
        }
        else {


        this.setState({
            table: {
                columns: [
                    {
                        label: 'Długość',
                        field: 'length',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Szerokość',
                        field: 'width',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Grubość',
                        field: 'thickness',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Kolor',
                        field: 'color',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Rodzaj',
                        field: 'type',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Ilość',
                        field: 'amount',
                        sort: 'asc',
                        width: 150
                    },
                    {
                        label: 'Usuń',
                        field: 'action',
                        sort: 'asc',
                        width: 150
                    }
                ],
                rows: this.state.table.rows.concat([{
                    width:  this.refs.width.value,
                    length: this.refs.length.value,
                    thickness: this.refs.thickness.value,
                    color: this.refs.color.value,
                    amount: this.refs.amount.value,
                    type: this.refs.type.value,
                   // delete: <button className="danger_t" id={i} onClick={(e) => { this.delete(table2[e.target.id].number, table2[e.target.id].deleted) }}> Usuń  </button>,
                    shape: 'rectangle',
                }]),
            },
            check:'ok'
        })
            console.log(this.state.table.rows)
        }
    }

    colorsSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.colors.length; i++) {

            tab.push(< option value={this.state.colors[i].color} > {this.state.colors[i].color}</option >)


        }
        return (tab)
    }
    typeSelector = (event) => {
        var tab = []
        for (var i = 0; i < this.state.type.length; i++) {

            tab.push(< option value={this.state.type[i].type} > {this.state.type[i].type}</option >)


        }
        return (tab)
    }

    table() {
        return (
            <MDBDataTableV5


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table}
                searchTop


                materialSearch
                searchBottom={false}
                // barReverse
                //  pagingTop
                // scrollX
                // scrollY
                responsive
                // maxHeight="35vh"
                bordered



                //   maxHeight="20vh"
                // borderless
                // btn
                // dark


                //maxHeight="400px"

                // paginationLabel={["<", ">"]}

                sortable


                // small
                // tego w ciemnym trybie nie ruszać/ striped/
                // theadColor="indigo"
                theadTextWhite
                // theadColor="indigo"
                theadTextWhite
                // barReverse
               // className="User_table"
                // noBottomColumns
                sortable
            //info={false}


            //   autoWidth


            />
        )
    }
    render() {
        let x = this.colorsSelector()
        let y = this.typeSelector()
        let table = this.table();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Zaloguj się, aby usyskać dostęp!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Logowanie</button>
                </div>
            );
        }
        else {
            return (
                <div>
                    <Sidebar />
                    <div className="AddOrder2">
                        <form>

                            <div className="form-group">
                                <h2>Dodawanie obiektu</h2>
                                <label>Długość</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Podaj wysokość"
                                    id="inputHeight"
                                    ref="length"
                                />
                            </div>
                            <div className="form-group">
                                <label>Szerokość</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Podaj szerokość"
                                    id="inputWidth"
                                    ref="width"
                                />
                            </div>
                            <div className="form-group">
                                <label>Grubość</label>
                                <input
                                    type="number"
                                    className="form-control"
                                    min="1"
                                    placeholder="Podaj grubość"
                                    id="inputThickness"
                                    ref="thickness"
                                />
                            </div>
                            <div className="form-group">
                                <label>Kolor</label>
                                <select ref="color" type="text" className="form-control">
                                    {x}
                                </select>
                            </div>
                            <div className="form-group">
                                <label>Ilość</label>
                                <input
                                    type="number"
                                    min="1"
                                    className="form-control"
                                    id="inputLogin"
                                    placeholder="Podaj lilość"
                                    ref="amount"
                                />
                            </div>
                            <div className="form-group">
                                <label>Rodzaj</label>
                                <select ref="type" type="text" className="form-control">
                                    {y}
                                </select>
                            </div>


                        </form>
                        <div className="form-group">
                            <button type="submit" className="success_order2_1" onClick={this.addItem}>Dodaj element</button>

                            <button type="submit" className="danger_order2" onClick={this.cancelAdding}>Anuluj zlecenie</button>


                        </div>
                        <div className="ordertable">
                            {table}
                        </div>
                        <button type="submit" className="success_order2_2" onClick={this.handleAddOrder}>Dodaj</button>
                    </div>

                </div>
            )
        }
    }

}