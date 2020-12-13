import React, { Component } from "react";
import './test.css';
import { OptiTable } from './OptiTable'



export class Test extends Component {
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2:'',
            table: [],

        }
    }

    componentDidMount() {
        const receiver = {
            order: {
                id_order: sessionStorage.getItem('idOpti'),
            },
            user: {
                login: sessionStorage.getItem('login'),
            },
            item: {
                color: sessionStorage.getItem('colorOpti'),
                type: sessionStorage.getItem('typeOpti'),
                thickness: sessionStorage.getItem('thicknessOpti'),

            }
        }
        fetch(`api/Cut/Magic`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json)
            })






        var canvas = document.getElementById('canvas');
        var x = 0;
        var y = 0;
        var w = 50;
        var h = 50;
        /////
        var x2 = 50;
        var y2 = 0;
        var w2 = 200;
        var h2 = 250;
        /////
        var x3 = 0;
        var y3 = 50;
        var w3 = 50;
        var h3 = 50;
        
        if (canvas.getContext) {
            var ctx = canvas.getContext('2d');
            //ctx.fillText("tekst", x, y);
            ctx.font = "40px Arial";
            ctx.fillText("450x450", 0, 40);
            ctx.font = "10px Arial";
            ctx.strokeRect(0, 50, 450, 450); ///tafla
            ctx.strokeRect(x, y+50, w, h);///itemy
            ctx.fillText("50x50", x+5, 60);
            ctx.strokeRect(x2, y2+50, w2, h2);
            ctx.fillText("200x250", x2 + 5, y2+60);
            ctx.strokeRect(x3, y3+50, w3, h3);
            ctx.fillText("50x50", x3 + 5, y3+60);
        }
    }

   


    

    render() {
        return (
            <div>
                <div className="canva">
                    <canvas className="canvas" id="canvas" width="600" height="600" ></canvas>
                </div>
                <div className="table2">
                    <OptiTable />
                </div>
                <div className="table3">
                    <OptiTable />
                </div>
            </div>
        );
    }
}

