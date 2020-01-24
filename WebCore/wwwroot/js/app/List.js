
(function(){

  function __init(target, callback){
    MAPA.Include('../../js/MAPA.Calendar.js', function(){      
      MAPA.Include('../../js/MAPA.Schedule.js', function(){
               
        var _current      = { dayData : '' };
        var _itemTemplate = $('itemTemplate');

        // ================================================================================================
        // Creación del contenido de las diferentes vistas de los datos
        // ================================================================================================          
        __builder = { 
          agenda : function(sender, date ){              
            if(!_current.dayData) return [];              
            return _current.dayData
                            .reduce(function(nodes, e){                                                          
                              nodes.push( MAPA.templates.fill(_itemTemplate.cloneNode(true), e) );
                              return nodes;
                            }, []);                                         
          },
          day :  function(sender, date ){
            if(!_current.dayData) return [];  

            var __tops = {};
            var __heights = {};                                     
            _current.dayData.forEach(function(e,i){ 
              __tops[e.id]    = '{0}px'.format(10 + (26 * i));
              __heights[e.id] = '15px';        
            }); 
              
            function __getRange(e){
              var a = e.d.fixTime().split(':');
              var b = e.d3.fixTime().split(':');
              return { S : ~~a[0] * 60 + ~~a[1], E : ~~b[0] * 60 + ~~b[1] };
            }
            return _current.dayData.reduce( function(nodes, e, i){
              var __size = sender.MeasureDayItem(__getRange(e));
              var __item = $.$('div', { className : 'Schedule_i', 
                                        style     : { left  : __size.Left,
                                                      width : '15px' || __size.Width,
                                                      top   : __tops[e.id],
                                                      height: '15px' || __heights[e.id]} });
              nodes.push(__item);
              return nodes
            }, []);
          },
          month : function(sender, date){
            var __days = Object.keys(MAPA.polSchedule.dataSource)
                             .reduce( function(a, p){
                                var __date = (p + ' 00:00:00').toDate('/', 'ddmmyyyy');
                                if(__date < sender.FirstDate) return a;
                                if(__date > sender.LastDate)  return a;
                                a.push( { fecha : p, count : MAPA.polSchedule.dataSource[p].length });
                                return a;
                             }, []);
            return __days.reduce( function(nodes, f){               
              nodes[f.fecha] = [];
              nodes[f.fecha].push($.$('div', {innerHTML : '{count} envíos'.format(f),
                                              id        : 'ctn_{fecha}'.format(f),
                                              onclick   : function(){ sender.ShowDayView(this.id.split('_')[1]); }
              }));
              return nodes;
            }, {});              
          },
        }
        // ==========================================================================================
        // Eventos
        // ========================================================================================== 
        function __onMonthChanged(sender, date){ __syncUI(sender); }
        function __onDayChanged  (sender, date){
          sender.ClearDayView();
          sender.ClearAgendaView();
          _current.dayData = MAPA.polSchedule.dataSource[date.format()];
          sender.LoadDayView(__builder.day(sender, date));
          sender.LoadAgendaView(__builder.agenda(sender, date));
        }
        function __syncUI(sender){
          sender.ClearMonthView();
          sender.LoadMonthView(__builder.month(sender, sender.Date)); 
          __onDayChanged(sender, sender.Date);
        }
        // ==========================================================================================
        // Creación del control
        // ========================================================================================== 
        callback( MAPA.Schedule({ Element        : target,
                                  View           : 'Month',
                                  Min            : '0',
                                  Max            : 24 * 60,  
                                  OnMonthChanged : __onMonthChanged,
                                  OnDayChanged   : __onDayChanged }));
        //_schedule.Footer.appendChild($('Leyenda')); 
        //_schedule.Footer.appendChild($('Footer_left'));
        //_schedule.Header.appendChild($('cmbSalaContainer')); 
      })
    })
  }
  
  MAPA.polSchedule = { init : __init };

}(MAPA));

;
